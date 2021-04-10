using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core
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
