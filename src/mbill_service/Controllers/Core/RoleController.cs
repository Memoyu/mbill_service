using AutoMapper;
using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Permission;
using mbill_service.Service.Core.Permission.Input;
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
        [LocalAuthorize("新增角色", "管理员")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyRoleDto dto)
        {
            await _roleService.InsertAsync(_mapper.Map<RoleEntity>(dto));
            return ServiceResult.Successed("角色创建成功");
        }

        /// <summary> 
        /// 删除角色
        /// </summary>
        /// <param name="id">角色id</param>
        [HttpDelete]
        [LocalAuthorize("删除角色", "管理员")]
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
        [LocalAuthorize("更新角色", "管理员")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> UpdateAsync([FromBody] ModifyRoleDto dto)
        {
            await _roleService.UpdateAsync(_mapper.Map<RoleEntity>(dto));
            return ServiceResult.Successed("角色更新成功！");
        }

        /// <summary>
        /// 获取全部角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [LocalAuthorize("获取全部角色", "管理员")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult<List<RoleDto>>> GetAllAsync()
        {
            var result = await _roleService.GetAllAsync();
            return ServiceResult<List<RoleDto>>.Successed(result);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [LocalAuthorize("角色详情", "管理员")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult<List<RoleDto>>> GetAsync(long id)
        {
            var result = await _roleService.GetAsync(id);
            return ServiceResult<List<RoleDto>>.Successed(result);
        }
    }
}
