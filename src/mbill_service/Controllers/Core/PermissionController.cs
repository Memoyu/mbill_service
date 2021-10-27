using mbill_service.Core.AOP.Attributes;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Service.Core.Permission;
using mbill_service.Service.Core.Permission.Input;
using mbill_service.Service.Core.Permission.Output;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Core
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Route("api/admin/permission")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public class PermissionController : ApiControllerBase
    {
        private readonly IPermissionSvc _permissionService;

        public PermissionController(IPermissionSvc permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 查询权限信息（树形结构）
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
        [LocalAuthorize("查询权限信息（树形结构）", "管理员")]
        public async Task<ServiceResult<List<TreePermissionDto>>> GetTreePermissions()
        {
            var result = await _permissionService.GetAllTreeAsync();
            return ServiceResult<List<TreePermissionDto>>.Successed(result);
        }

        /// <summary>
        /// 查询所有可分配的权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("module")]
        [LocalAuthorize("查询所有可分配的权限", "管理员")]
        public async Task<ServiceResult<IDictionary<string, IEnumerable<PermissionDto>>>> GetModulePermissions()
        {
            var result = await _permissionService.GetAllStructualAsync();
            return ServiceResult<IDictionary<string, IEnumerable<PermissionDto>>>.Successed(result);
        }

        /// <summary>
        /// 配置角色权限
        /// </summary>
        /// <param name="dto">角色权限</param>
        [Logger("配置角色权限")]
        [HttpPost("dispatch")]
        [LocalAuthorize("配置角色权限", "管理员")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> DispatchPermissions([FromBody] DispatchPermissionsDto dto)
        {
            await _permissionService.DispatchPermissionsAsync(dto);
            return ServiceResult.Successed("配置角色权限成功");
        }
    }
}
