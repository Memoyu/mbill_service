namespace Memo.Bill.Application.Roles.Commands.Create;

public class CreateRoleCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Role> roleRepo,
    IBaseDefaultRepository<Permission> permissionRepo,
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : IRequestHandler<CreateRoleCommand, Result>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var exist = await roleRepo.Select.AnyAsync(r => r.Name == request.Name, cancellationToken);
        if (exist) throw new ApplicationException("同名角色已存在");

        var permission = await permissionRepo.Select.Where(p => request.Permissions.Contains(p.PermissionId)).ToListAsync(cancellationToken);
        foreach (var permissionId in request.Permissions)
        {
            if (!permission.Any(t => t.PermissionId == permissionId)) throw new ApplicationException($"{permissionId}权限不存在");
        }

        var role = mapper.Map<Role>(request);
        role = await roleRepo.InsertAsync(role, cancellationToken);
        if (role.Id <= 0) throw new ApplicationException("保存角色失败");

        var rolePermissions = request.Permissions.Select(p => new RolePermission { RoleId = role.RoleId, PermissionId = p }).ToList();
        await rolePermissionRepo.InsertAsync(rolePermissions, cancellationToken);

        return Result.Success(role.RoleId);
    }
}
