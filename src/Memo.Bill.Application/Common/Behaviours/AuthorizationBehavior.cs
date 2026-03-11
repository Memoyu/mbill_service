using System.Reflection;

namespace Memo.Bill.Application.Common.Behaviours;

public class AuthorizationBehavior<TRequest, TResponse>(IAuthorizationService _authorizationService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizeableRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        var authorizationResult = await _authorizationService.AuthorizeCurrentUserAsync(
            request,
            requiredPermissions);

        return authorizationResult.IsSuccess
            ? await next()
            : (dynamic)authorizationResult;
    }
}
