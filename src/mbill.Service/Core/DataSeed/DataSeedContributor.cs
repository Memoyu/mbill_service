namespace Mbill.Service.Core.DataSeed;

public class DataSeedContributor : IDataSeedContributor, ISingletonDependency
{

    private readonly IPermissionRepo _permissionRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;
    private readonly ILogger<DataSeedContributor> _logger;
    public DataSeedContributor(IPermissionRepo permissionRepo, IRolePermissionRepo rolePermissionRepo, ILogger<DataSeedContributor> logger)
    {
        _permissionRepo = permissionRepo;
        _rolePermissionRepo = rolePermissionRepo;
        _logger = logger;
    }

    public async Task InitAdministratorPermissionAsync()
    {
        //bool valid = await _rolePermissionRepo.Select.AnyAsync();
        //if (!valid) return;
        //List<PermissionEntity> allPermissions = await _permissionRepo.Select.ToListAsync();//获取所有权限
        //List<RolePermissionEntity> rolePermissions = allPermissions.Select(u => new RolePermissionEntity(SystemConst.Role.Administrator, u.Id)).ToList();//构建超级管理员角色权限
        //await _rolePermissionRepo.InsertAsync(rolePermissions);//插入全部的超级管理员角色权限
    }

    public async Task InitPermissionAsync(List<PermissionDefinition> permissions)
    {
        List<PermissionEntity> insertPermissions = new List<PermissionEntity>();//新增权限集合
        List<PermissionEntity> updatePermissions = new List<PermissionEntity>();//更新权限集合

        Expression<Func<RolePermissionEntity, bool>> rolePermissionExpression = u => false;
        Expression<Func<PermissionEntity, bool>> permissionExpression = u => false;

        List<PermissionEntity> allPermissions = await _permissionRepo.Select.ToListAsync();//已持久化的权限数据

        allPermissions.ForEach(per =>//过滤已持久化的权限数据，获得需要删除的权限、角色权限数据
        {
            if (permissions.All(r => r.Permission != per.Name))//持久化的权限数据是否存在于在本次获取到的权限数据
            {
                permissionExpression = permissionExpression.Or(r => r.Id == per.Id);//拼接表达式，权限Id
                rolePermissionExpression = rolePermissionExpression.Or(r => r.PermissionId == per.Id);//拼接表达式，角色权限Id
            }
        });

        int effectPerRows = await _permissionRepo.DeleteAsync(permissionExpression);//删除权限数据
        int effectRolePerRows = await _rolePermissionRepo.DeleteAsync(rolePermissionExpression);//删除角色权限数据
        _logger.LogInformation($"操 作 权 限 表：删除了{effectPerRows}条数据");
        _logger.LogInformation($"操作角色权限表：删除了{effectRolePerRows}条数据");

        permissions.ForEach(per =>//过滤本次获取到的权限数据，获得需要新增、更新的权限数据
        {
            PermissionEntity permissionEntity = allPermissions.FirstOrDefault(u => u.Module == per.Module && u.Name == per.Permission);//在已持久化的权限数据中获取符合条件数据
            if (permissionEntity == null)//如果权限数据为空，则可新增
            {
                insertPermissions.Add(new PermissionEntity(per.Permission, per.Module, per.Router));
            }
            else//否则
            {
                bool routerExist = allPermissions.Any(u => u.Module == per.Module && u.Name == per.Permission && u.Router == per.Router);//是否存在符合条件的数据
                if (!routerExist)//不存在则证明Router发生了改变，则更新Router，
                {
                    permissionEntity.Router = per.Router;
                    updatePermissions.Add(permissionEntity);
                }
            }
        });

        var inserts = await _permissionRepo.InsertAsync(insertPermissions);
        _logger.LogInformation($"操 作 权 限 表：新增了{inserts.Count()}条数据");

        var updateCount = await _permissionRepo.UpdateAsync(updatePermissions);
        _logger.LogInformation($"操 作 权 限 表：更新了{updateCount}条数据");

    }
}
