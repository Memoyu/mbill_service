using AutoMapper;
using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Permission;
using mbill_service.Service.Core.Permission.Input;
using mbill_service.Service.Core.Permission.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Core
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/admin/role")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public class RoleController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        [Logger("新建一个角色")]
        [HttpPost]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyRoleDto dto)
        {
            await _roleService.InsertAsync(dto);
            return ServiceResult.Successed("角色创建成功");
        }

        /// <summary> 
        /// 删除角色
        /// </summary>
        /// <param name="id">角色id</param>
        [HttpDelete]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> DeleteAsync([FromQuery] long id)
        {
            await _roleService.DeleteAsync(id);
            return ServiceResult.Successed("角色删除成功！");
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        [HttpPut]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> UpdateAsync([FromBody] ModifyRoleDto dto)
        {
            await _roleService.UpdateAsync(dto);
            return ServiceResult.Successed("角色更新成功！");
        }

        /// <summary>
        /// 获取全部角色信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("all")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult<List<RoleDto>>> GetAllAsync()
        {
            return ServiceResult<List<RoleDto>>.Successed(await _roleService.GetAllAsync());
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult<RolePermissionDto>> GetAsync(long id)
        {
            return ServiceResult<RolePermissionDto>.Successed(await _roleService.GetAsync(id));
        }
    }
}
