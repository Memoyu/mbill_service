namespace Memo.Bill.Application.Common.Interfaces.Security;

public interface IAuthorizationService
{
    Task<Result> AuthorizeCurrentUserAsync<T>(IAuthorizeableRequest<T> request, List<string> requiredPermissions);
}
