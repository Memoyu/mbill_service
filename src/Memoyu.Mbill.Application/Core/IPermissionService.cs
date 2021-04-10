/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Core.Permission
*   文件名称 ：PermissionService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-14 22:45:50
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core
{
    public interface IPermissionService
    {
        /// <summary>
        /// 获取全部权限(结构化)
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<string, IEnumerable<PermissionDto>>> GetAllStructual();

        /// <summary>
        /// 检查当前登陆用户的分组权限
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<bool> CheckAsync(string permission);
    }
}
