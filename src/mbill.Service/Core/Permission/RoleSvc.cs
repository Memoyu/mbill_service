namespace Mbill.Service.Core.Permission;

public class RoleSvc : ApplicationSvc, IRoleSvc
{
    private readonly IRoleRepo _roleRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;
    private readonly IPermissionRepo _permissionRepo;
    public RoleSvc(IRoleRepo roleRepo, IRolePermissionRepo rolePermissionRepo, IPermissionRepo permissionRepo)
    {
        _roleRepo = roleRepo;
        _rolePermissionRepo = rolePermissionRepo;
        _permissionRepo = permissionRepo;
    }


    public async Task<List<RoleDto>> GetAllAsync()
    {
        var entitys = await _roleRepo.Select.Where(r => r.IsDeleted == false).ToListAsync();
        var dtos = entitys.Select(e => Mapper.Map<RoleDto>(e)).ToList();
        return dtos;
    }

    [Transactional]
    public async Task InsertAsync(ModifyRoleDto role)
    {
        bool isRepeatName = await _roleRepo.Select.AnyAsync(r => r.Name == role.Name);
        if (isRepeatName)//角色名重复
            throw new KnownException("角色名称重复，请重新输入", ServiceResultCode.RepeatField);
        var permissions = await _permissionRepo.Select.Where(p => p.IsDeleted == false).ToListAsync();
        foreach (var permissionId in role.PermissionIds)
        {
            if (!permissions.Any(p => p.Id == permissionId))
                throw new KnownException($"Id:{permissionId} 权限不存在！", ServiceResultCode.NotFound);
        }

        var input = Mapper.Map<RoleEntity>(role);
        var entity = await _roleRepo.InsertAsync(input);
        await _rolePermissionRepo.InsertAsync(role.PermissionIds.Select(p => new RolePermissionEntity
        {
            RoleId = entity.Id,
            PermissionId = p
        }));
    }

    [Transactional]
    public async Task DeleteAsync(long id)
    {
        throw new System.NotImplementedException();
        //var dto = await GetAsync(id);
        //await _rolePermissionRepo.DeleteAsync(entity)
    }

    public Task UpdateAsync(ModifyRoleDto role)
    {
        throw new System.NotImplementedException();
    }

    public async Task<RolePermissionDto> GetAsync(long id)
    {
        var role = await _roleRepo
            .Select
            .IncludeMany(r => r.RolePermissions)
            .Where(r => r.IsDeleted == false)
            .Where(r => r.Id == id).FirstAsync();
        if (role == null)
            throw new KnownException("角色不存在！", ServiceResultCode.NotFound);
        var dto = Mapper.Map<RolePermissionDto>(role);
        return dto;

    }
}