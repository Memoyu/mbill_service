namespace mbill.Modules.Configs;

public class PermissionAuthorizationHandler : AuthorizationHandler<ModuleAuthorizationRequirement>
{
    private readonly IPermissionSvc _permissionService;

    public PermissionAuthorizationHandler(IPermissionSvc permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ModuleAuthorizationRequirement requirement)
    {
        AuthorizationFilterContext? filterContext = context.Resource as AuthorizationFilterContext;

        if (!context.User.Identity.IsAuthenticated)
        {
            HandlerAuthenticationFailed(filterContext, "认证失败，请检查请求头或者重新登陆", ServiceResultCode.AuthenticationFailed);
            context.Fail();
            return;
        }

        if (await _permissionService.CheckAsync(requirement.Module, requirement.Name))
        {
            context.Succeed(requirement);
            return;
        }
        HandlerAuthenticationFailed(filterContext, $"您没有权限：{requirement.Module}-{requirement.Name}", ServiceResultCode.NoPermission);
    }

    public void HandlerAuthenticationFailed(AuthorizationFilterContext context, string message, ServiceResultCode code)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Result = new JsonResult(new ServiceResult(code, message));
    }
}
