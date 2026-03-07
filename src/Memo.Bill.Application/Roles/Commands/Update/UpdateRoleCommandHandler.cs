namespace Memo.Bill.Application.Roles.Commands.Update;

public class UpdateRoleCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Role> roleRepo,
    IBaseDefaultRepository<Permission> permissionRepo,
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : IRequestHandler<UpdateRoleCommand, Result>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await roleRepo.Select.Where(r => r.RoleId == request.RoleId).FirstAsync(cancellationToken) ?? throw new ApplicationException("角色不存在");
        var permissions = await permissionRepo.Select.Where(p => request.Permissions.Contains(p.PermissionId)).ToListAsync();
        foreach (var permissioId in request.Permissions)
        {
            if (!permissions.Any(t => t.PermissionId == permissioId)) throw new ApplicationException($"{permissioId}权限不存在");
        }

        var role = mapper.Map<Role>(request);
        role.Id = entity.Id;
        var affrows = await roleRepo.UpdateAsync(role, cancellationToken);
        if (affrows <= 0) throw new ApplicationException("更新角色失败");

        #region 角色关联权限管理

        var addRolePermissions = new List<RolePermission>();
        var currentRolePermissions = await rolePermissionRepo.Select.Where(at => at.RoleId == role.RoleId).ToListAsync(cancellationToken);
        foreach (var permission in permissions)
        {
            if (!currentRolePermissions.Any(rp => rp.PermissionId == permission.PermissionId))
            {
                addRolePermissions.Add(new RolePermission { PermissionId = permission.PermissionId, RoleId = request.RoleId });
            }
            else
            {
                currentRolePermissions.RemoveAll(t => t.PermissionId == permission.PermissionId);
            }
        }
        addRolePermissions= await rolePermissionRepo.InsertAsync(addRolePermissions, cancellationToken);
        if (addRolePermissions.Any(ur => ur.Id <= 0)) throw new ApplicationException("添加角色权限失败");
        var delRolePermissionAffrows = await rolePermissionRepo.DeleteAsync(currentRolePermissions, cancellationToken);
        if (delRolePermissionAffrows != currentRolePermissions.Count) throw new ApplicationException("删除角色权限失败");

        #endregion

        return Result.Success(role.RoleId);
    }
}
