using mbill_service.Service.Core.Permission.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
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
