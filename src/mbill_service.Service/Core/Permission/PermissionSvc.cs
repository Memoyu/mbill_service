namespace mbill_service.Service.Core.Permission;
public class PermissionSvc : ApplicationSvc, IPermissionSvc
{
    private readonly IPermissionRepo _permissionRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;

    public PermissionSvc(IPermissionRepo permissionRepo, IRolePermissionRepo rolePermissionRepo)
    {
        _permissionRepo = permissionRepo;
        _rolePermissionRepo = rolePermissionRepo;
    }

    public async Task<List<TreePermissionDto>> GetAllTreeAsync()
    {
        var permissions = await _permissionRepo.Select.ToListAsync();
        int index = 1;
        List<TreePermissionDto> treePermissionDtos = permissions.GroupBy(r => r.Module).Select(r =>
                  new TreePermissionDto
                  {
                      Rowkey = index++.ToString(),
                      Children = permissions.Where(u => u.Module == r.Key)
                                            .Select(r => new TreePermissionDto
                                            {
                                                Id = r.Id,
                                                Rowkey = index++.ToString(),
                                                Name = r.Name,
                                                Router = r.Router,
                                                CreateTime = r.CreateTime
                                            })
                                            .ToList(),
                      Name = r.Key,
                  }).ToList();
        return treePermissionDtos;
    }

    public async Task<IDictionary<string, IEnumerable<PermissionDto>>> GetAllStructualAsync()
    {
        return (await _permissionRepo.Select.ToListAsync())
               .GroupBy(r => r.Module)
               .ToDictionary(
                   group => group.Key,
                   group =>
                       Mapper.Map<IEnumerable<PermissionDto>>(group.ToList())
               );
    }

    public async Task<bool> CheckAsync(string permission)
    {
        long[] roleIds = CurrentUser.Roles;
        PermissionEntity permissionEntity = await _permissionRepo.Where(r => r.Name == permission).FirstAsync();
        bool existPermission = await _rolePermissionRepo.Select
            .AnyAsync(r => roleIds.Contains(r.RoleId) && r.PermissionId == permissionEntity.Id);
        return existPermission;
    }

    [Transactional]
    public async Task<bool> DispatchPermissionsAsync(DispatchPermissionsDto dto)
    {
        //去重
        var distinctPers = dto.PermissionIds.Distinct().ToList();

        var pers = await _permissionRepo.Select.ToListAsync();
        var notExist = distinctPers.Where(p => !pers.Any(per => per.Id == p));
        if (notExist.Any()) throw new KnownException($"Id：{string.Join(",", notExist)} 的权限不存在！", ServiceResultCode.NotFound, 200);
        var rolePers = await _rolePermissionRepo.Select.Where(rp => rp.RoleId == dto.RoleId).ToListAsync();
        //需要清除的权限   
        var deletePers = rolePers.Where(r => !distinctPers.Any(p => p == r.PermissionId)).ToList();
        //需要新增的权限
        var addPers = distinctPers.Where(p => !rolePers.Any(r => r.PermissionId == p)).Select(p => new RolePermissionEntity
        {
            RoleId = dto.RoleId,
            PermissionId = p
        }).ToList();
        if (deletePers.Count > 0)
            await _rolePermissionRepo.DeleteAsync(deletePers);
        if (addPers.Count > 0)
            await _rolePermissionRepo.InsertAsync(addPers);

        return true;
    }
}