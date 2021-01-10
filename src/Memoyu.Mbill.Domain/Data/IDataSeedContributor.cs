/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Data
*   文件名称 ：IDataSeedContributor.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 15:00:51
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Shared.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Data
{
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
}
