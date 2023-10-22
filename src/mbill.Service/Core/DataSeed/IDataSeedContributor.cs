namespace Mbill.Service.Core.DataSeed;

public interface IDataSeedContributor
{
    /// <summary>
    /// 初始化超级管理员角色权限
    /// </summary>
    /// <returns></returns>
    Task InitAdministratorPermissionAsync();

    /// <summary>
    /// 初始化权限，根据PermissionAttribute生成权限，改变的则更新，否则新增
    /// </summary>
    /// <param name="permissions"></param>
    /// <returns></returns>
    Task InitPermissionAsync(List<PermissionDefinition> permissions);
}
