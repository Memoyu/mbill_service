using Mbill.Core.Common;

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

    public async Task<ServiceResult<List<RoleDto>>> GetAllAsync()
    {
        var entitys = await _roleRepo.Select.ToListAsync();
        var dtos = entitys.Select(Mapper.Map<RoleDto>).ToList();
        return ServiceResult<List<RoleDto>>.Successed(dtos);
    }

    [Transactional]
    public async Task<ServiceResult> InsertAsync(ModifyRoleDto role)
    {
        bool isRepeatName = await _roleRepo.Select.AnyAsync(r => r.Name == role.Name);
        if (isRepeatName)//角色名重复
            throw new KnownException("角色名称重复，请重新输入", ServiceResultCode.RepeatField);
        var permissions = await _permissionRepo.Select.ToListAsync();
        foreach (var permissionBId in role.PermissionBIds)
        {
            if (!permissions.Any(p => p.BId == permissionBId))
                throw new KnownException($"Id:{permissionBId} 权限不存在！", ServiceResultCode.NotFound);
        }

        var entity = Mapper.Map<RoleEntity>(role);
        entity.BId = SnowFlake.NextId();
        entity = await _roleRepo.InsertAsync(entity);
        await _rolePermissionRepo.InsertAsync(role.PermissionBIds.Select(pBId => new RolePermissionEntity
        {
            BId = SnowFlake.NextId(),
            RoleBId = entity.BId,
            PermissionBId = pBId
        }));

        return ServiceResult.Successed("新增角色成功");
    }

    [Transactional]
    public async Task<ServiceResult> DeleteAsync(long id)
    {
        throw new System.NotImplementedException();
        //var dto = await GetAsync(id);
        //await _rolePermissionRepo.DeleteAsync(entity)
    }

    public Task<ServiceResult> UpdateAsync(ModifyRoleDto role)
    {
        throw new System.NotImplementedException();
    }

    public async Task<ServiceResult<RoleWithPermissionDto>> GetAsync(long id)
    {
        var role = await _roleRepo
            .Select
            .IncludeMany(r => r.RolePermissions)
            .Where(r => r.Id == id).FirstAsync();
        if (role == null)
            throw new KnownException("角色不存在！", ServiceResultCode.NotFound);
        var dto = Mapper.Map<RoleWithPermissionDto>(role);
        return ServiceResult<RoleWithPermissionDto>.Successed(dto);

    }
}