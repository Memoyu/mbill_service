using mbill_service.Service.Core.Permission.Input;
using mbill_service.Service.Core.Permission.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
{
    public interface IPermissionService
    {
        /// <summary>
        /// 获取所有权限(树形结构)
        /// </summary>
        /// <returns></returns>
        Task<List<TreePermissionDto>> GetAllTreeAsync();

        /// <summary>
        /// 获取所有权限(Module分组结构化)
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<string, IEnumerable<PermissionDto>>> GetAllStructualAsync();

        /// <summary>
        /// 检查当前登陆用户的分组权限
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<bool> CheckAsync(string permission);
    }
}
