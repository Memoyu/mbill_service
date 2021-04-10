/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Controllers.Core
*   文件名称 ：PermissionController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-14 22:36:43
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Application.Contracts.Filter;
using Memoyu.Mbill.Application.Core;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.Core
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
        [HttpGet]
        [PermissionAuthorize("查询所有可分配的权限", "管理员")]
        public async Task<ServiceResult<IDictionary<string, IEnumerable<PermissionDto>>>> GetAllPermissions()
        {
            var result = await _permissionService.GetAllStructual();
            return ServiceResult<IDictionary<string, IEnumerable<PermissionDto>>>.Successed(result);
        }
    }
}
