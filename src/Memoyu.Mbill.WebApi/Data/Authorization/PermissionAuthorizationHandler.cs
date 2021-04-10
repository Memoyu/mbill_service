/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Data.Authorization
*   文件名称 ：PermissionAuthorizationHandler.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-15 0:03:58
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Data.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizationHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            Claim userIdClaim = context.User?.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                if (await _permissionService.CheckAsync(requirement.Name))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
