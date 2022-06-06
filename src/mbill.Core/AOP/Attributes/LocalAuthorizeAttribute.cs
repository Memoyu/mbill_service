namespace mbill.Core.AOP.Attributes;

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
        //ICurrentUser currentUser = (ICurrentUser)context.HttpContext.RequestServices.GetService(typeof(ICurrentUser));

        //if (currentUser.IsInGroup(SystemConst.Role.Administrator))//如果是超级管理员
        //{
        //    return;
        //}

        IAuthorizationService authorizationService = (IAuthorizationService)context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService));
        AuthorizationResult authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new ValidJtiRequirement());
        if (!authorizationResult.Succeeded)
        {
            return;
        }

        await authorizationService.AuthorizeAsync(context.HttpContext.User, context, new ModuleAuthorizationRequirement(Module, Permission));
    }

    public override string ToString()
    {
        return $"\"{base.ToString()}\",\"Permission:{Permission}\",\"Module:{Module}\",";
    }
}
