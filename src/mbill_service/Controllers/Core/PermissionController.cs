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
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 查询所有可分配的权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
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
    }
}
