namespace mbill_service.Modules.Configs;

public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
{
    private readonly IPermissionSvc _permissionService;

    public PermissionAuthorizationHandler(IPermissionSvc permissionService)
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
