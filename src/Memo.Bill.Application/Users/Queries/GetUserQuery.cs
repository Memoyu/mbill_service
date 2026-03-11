using Memo.Bill.Application.Common.Security;
using Memo.Bill.Application.Roles.Common;
using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Users.Queries;

public  record GetUserQuery(long? UserId) : IAuthorizeableRequest<Result>;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
}

public class GetUserQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserRole> userRoleRepo,
     IBaseDefaultRepository<UserIdentity> userIdentityRepo
    ) : IRequestHandler<GetUserQuery, Result>
{
    public async Task<Result> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        // 没有传入用户Id时从认证信息中获取Id
        if (!request.UserId.HasValue)
            userId = currentUserProvider.GetCurrentUser().Id;

        if (userId <= 0) throw new ApplicationException("当前用户Id为空");

        var user = await userRepo.Select.Where(u => u.UserId == userId).FirstAsync(cancellationToken) ?? throw new ApplicationException("用户不存在");
        var userRoles = await userRoleRepo.Select
            .Include(u => u.Role)
            .Where(u => u.UserId == userId).ToListAsync(cancellationToken) ?? [];

        var userIdentity = await userIdentityRepo.Select.Where(ui => ui.UserId == userId).FirstAsync(cancellationToken);

        var dto = mapper.Map<UserWithUserIdentityResult>(user);
        dto.Roles = userRoles.Select(ur => mapper.Map<RoleListResult>(ur.Role)).ToList();
        dto.UserIdentity = mapper.Map<UserIdentityResult>(userIdentity);

        return Result.Success(dto);
    }
}

