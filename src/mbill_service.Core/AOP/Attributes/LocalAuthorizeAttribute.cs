using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace mbill_service.Core.AOP.Attributes
{
    /// <summary>
    ///  自定义固定权限编码给动态角色及用户，支持验证登录，指定角色、Policy
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class LocalAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string Permission { get; }
        public string Module { get; }

        public LocalAuthorizeAttribute(string permission, string module)
        {
            Permission = permission;
            Module = module;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ClaimsPrincipal claimsPrincipal = context.HttpContext.User;

            if (!claimsPrincipal.Identity.IsAuthenticated)//认证失败
            {
                HandlerAuthenticationFailed(context, "认证失败，请检查请求头或者重新登陆", ServiceResultCode.AuthenticationFailed);
                return;
            }

            ICurrentUser currentUser = (ICurrentUser)context.HttpContext.RequestServices.GetService(typeof(ICurrentUser));

            if (currentUser.IsInGroup(SystemConst.Role.Administrator))//如果是超级管理员
            {
                return;
            }

            IAuthorizationService authorizationService = (IAuthorizationService)context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService));
            AuthorizationResult authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new OperationAuthorizationRequirement() { Name = Permission });
            if (!authorizationResult.Succeeded)
            {
                HandlerAuthenticationFailed(context, $"您没有权限：{Module}-{Permission}", ServiceResultCode.NoPermission);
            }
        }

        public void HandlerAuthenticationFailed(AuthorizationFilterContext context, string message, ServiceResultCode code)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new JsonResult(new ServiceResult(code, message));
        }

        public override string ToString()
        {
            return $"\"{base.ToString()}\",\"Permission:{Permission}\",\"Module:{Module}\",";
        }
    }
}
