using Mbill.Core.Common;

namespace Mbill.Service.Core.DataSeed;

public class DataSeedSvc : ApplicationSvc, IDataSeedSvc
{
    private readonly ILogger<DataSeedSvc> _logger;
    private readonly IPermissionRepo _permissionRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;
    private readonly IRoleRepo _roleRepo;
    private readonly IDataSeedRepo _dataSeedRepo;

    public DataSeedSvc(
        ILogger<DataSeedSvc> logger,
        IPermissionRepo permissionRepo,
        IRolePermissionRepo rolePermissionRepo,
        IRoleRepo roleRepo,
        IDataSeedRepo dataSeedRepo
        )
    {
        _logger = logger;
        _permissionRepo = permissionRepo;
        _rolePermissionRepo = rolePermissionRepo;
        _roleRepo = roleRepo;
        _dataSeedRepo = dataSeedRepo;
    }

    public async Task InitDataSeedAsync()
    {
        var ds = await _dataSeedRepo.Select.ToListAsync();
        var types = Enum.GetValues(typeof(DataSeedType)).Cast<DataSeedType>().ToList();
        var adds = new List<DataSeedEntity>();
        foreach (var type in types)
        {
            if (ds.Any(d => d.Type == type.GetHashCode())) continue;
            adds.Add(new DataSeedEntity
            {
                BId = SnowFlake.NextId(),
                Type = type.GetHashCode(),
                Desc = type.GetCustomAttributeDescription(),
            });
        }

        // 插入数据
        await _dataSeedRepo.InsertAsync(adds);

        _logger.LogInformation($"完成种子数据初始化！");
    }

    public async Task InitAdministratorPermissionAsync()
    {
        var roles = await _roleRepo.Select.Where(r => r.Type == RoleType.Administrator.GetHashCode()).ToListAsync();
        if (!roles.Any()) return;

        var roleBIds = roles.Select(r => r.BId).ToList();
        List<PermissionEntity> pers = await _permissionRepo.Select.ToListAsync();//获取所有权限
        List<RolePermissionEntity> rolePers = await _rolePermissionRepo.Select.Where(rp => roleBIds.Contains(rp.RoleBId)).ToListAsync();
        var needAddRolePers = new List<RolePermissionEntity>();
        foreach (var role in roles)
        {
            var currRolePers = rolePers.Where(rp => rp.RoleBId == role.BId).ToList();
            var adds = pers.Where(p => !currRolePers.Any(crp => crp.PermissionBId == p.BId)).Select(p => new RolePermissionEntity
            {
                BId = SnowFlake.NextId(),
                RoleBId = role.BId,
                PermissionBId = p.BId,
            }).ToList();
            if (adds.Any())
                needAddRolePers.AddRange(adds);
        }

        if (needAddRolePers.Any())
            await _rolePermissionRepo.InsertAsync(needAddRolePers);//插入全部的超级管理员角色权限

        _logger.LogInformation($"超级管理员权限：新增了{needAddRolePers.Count}条数据");
    }

    public async Task InitPermissionAsync(List<PermissionDefinition> defPers)
    {
        List<PermissionEntity> insertPers = new List<PermissionEntity>();//新增权限集合
        List<PermissionEntity> updatePers = new List<PermissionEntity>();//更新权限集合

        Expression<Func<RolePermissionEntity, bool>> rolePermissionExpression = u => false;
        Expression<Func<PermissionEntity, bool>> permissionExpression = u => false;

        List<PermissionEntity> pers = await _permissionRepo.Select.ToListAsync();//已持久化的权限数据

        pers.ForEach(per =>//过滤已持久化的权限数据，获得需要删除的权限、角色权限数据
        {
            if (defPers.All(r => r.Permission != per.Name))//持久化的权限数据是否存在于在本次获取到的权限数据
            {
                permissionExpression = permissionExpression.Or(r => r.BId == per.BId);//拼接表达式，权限Id
                rolePermissionExpression = rolePermissionExpression.Or(r => r.PermissionBId == per.BId);//拼接表达式，角色权限Id
            }
        });

        int effectPerRows = await _permissionRepo.DeleteAsync(permissionExpression);//删除权限数据
        int effectRolePerRows = await _rolePermissionRepo.DeleteAsync(rolePermissionExpression);//删除角色权限数据
        _logger.LogInformation($"操 作 权 限 表：删除了{effectPerRows}条数据");
        _logger.LogInformation($"操作角色权限表：删除了{effectRolePerRows}条数据");

        defPers.ForEach(per =>//过滤本次获取到的权限数据，获得需要新增、更新的权限数据
        {
            PermissionEntity perEntity = pers.FirstOrDefault(u => u.Module == per.Module && u.Name == per.Permission);//在已持久化的权限数据中获取符合条件数据
            if (perEntity == null)//如果权限数据为空，则可新增
            {
                insertPers.Add(new PermissionEntity(per.Permission, per.Module, per.Router));
            }
            else//否则
            {
                bool routerExist = pers.Any(u => u.Module == per.Module && u.Name == per.Permission && u.Router == per.Router);//是否存在符合条件的数据
                if (!routerExist)//不存在则证明Router发生了改变，则更新Router，
                {
                    perEntity.Router = per.Router;
                    updatePers.Add(perEntity);
                }
            }
        });

        var inserts = await _permissionRepo.InsertAsync(insertPers);
        _logger.LogInformation($"操 作 权 限 表：新增了{inserts.Count()}条数据");

        var updateCount = await _permissionRepo.UpdateAsync(updatePers);
        _logger.LogInformation($"操 作 权 限 表：更新了{updateCount}条数据");

    }
}
