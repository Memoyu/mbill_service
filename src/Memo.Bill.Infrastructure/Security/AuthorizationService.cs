using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Infrastructure.Security;

public class AuthorizationService(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<UserRole> userRoleRepo,
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : IAuthorizationService
{
    public async Task<Result> AuthorizeCurrentUserAsync<T>(IAuthorizeableRequest<T> request,List<string> requiredPermissions)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var userRoles = await userRoleRepo.Select.Where(ur => ur.UserId == currentUser.Id).ToListAsync();
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
        var rolePermissions = await rolePermissionRepo.Select
            .Include(rp => rp.Permission)
            .Where(rp => rp.Permission != null && roleIds.Contains(rp.RoleId))
            .ToListAsync(rp => rp.Permission!.Signature);

        if (requiredPermissions.Except(rolePermissions).Any())
        {
            return Result.Failure( "当前用户无权发起该操作", ResultCode.Forbidden);
        }

        return Result.Success();
    }
}
