﻿using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Permission.Input;
using mbill_service.Service.Core.Permission.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Permission
{
    public interface IRoleService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        Task InsertAsync(ModifyRoleDto role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        Task DeleteAsync(long id);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role">账单分类信息</param>
        /// <returns></returns>
        Task UpdateAsync(ModifyRoleDto role);

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        Task<List<RoleDto>> GetAllAsync();

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        Task<RolePermissionDto> GetAsync(long id);
    }
}
