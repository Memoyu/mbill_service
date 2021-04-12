using mbill_service.Service.Core.Permission.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
{
    public interface IRoleService
    {
        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        Task<List<RoleDto>> GetAllAsync();
    }
}
