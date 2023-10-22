namespace Mbill.Modules.Configs;

public class ValidJtiHandler : AuthorizationHandler<ValidJtiRequirement>
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ValidJtiHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidJtiRequirement requirement)
    {
        //检查是否登录
        AuthorizationFilterContext filterContext = context.Resource as AuthorizationFilterContext;
        DefaultHttpContext defaultHttpContext = context.Resource as DefaultHttpContext;
        if (!context.User.Identity.IsAuthenticated)
        {
            HandlerAuthenticationFailed(filterContext, "认证失败，请检查请求头或者重新登陆", ServiceResultCode.AuthenticationFailed);
            context.Fail();
            return;
        }

        await Task.CompletedTask;
        context.Succeed(requirement);
    }

    public void HandlerAuthenticationFailed(AuthorizationFilterContext filterContext, string meessage, ServiceResultCode code)
    {
        if (filterContext == null) return;
        filterContext.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        filterContext.Result = new JsonResult(new ServiceResult(code, meessage));
    }
}
