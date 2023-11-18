namespace Mbill.Service.Core.DataSeed;

public interface IDataSeedSvc : IApplicationSvc
{
    /// <summary>
    /// 初始化已知类型的种子数据
    /// </summary>
    /// <returns></returns>
    Task InitDataSeedAsync();

    /// <summary>
    /// 初始化权限，根据PermissionAttribute生成权限，改变的则更新，否则新增
    /// </summary>
    /// <param name="defPermissions"></param>
    /// <returns></returns>
    Task InitPermissionAsync(List<PermissionDefinition> defPermissions);
    
    /// <summary>
    /// 初始化超级管理员角色权限
    /// </summary>
    /// <returns></returns>
    Task InitAdministratorPermissionAsync();

    /// <summary>
    /// 初始化普通用户角色权限
    /// </summary>
    /// <returns></returns>
    Task InitUserPermissionAsync();
}
