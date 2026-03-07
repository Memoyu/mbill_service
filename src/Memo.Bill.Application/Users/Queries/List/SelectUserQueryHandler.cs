using Memo.Bill.Application.Common.Security;
using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Users.Queries.List;

public class SelectUserQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<User> userRepo
    ) : IRequestHandler<SelectUserQuery, Result>
{
    public async Task<Result> Handle(SelectUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var users = await userRepo.Select
            .Where(u => u.UserId != userId)
            .WhereIf(!string.IsNullOrWhiteSpace(request.Username), u => u.Username.Contains(request.Username!))
            .WhereIf(!string.IsNullOrWhiteSpace(request.Nickname), u => u.Nickname.Contains(request.Nickname!))
            .OrderByDescending(u => u.CreateTime)
            .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<SelectUserResult>>(users);
        
        return Result.Success(dtos);
    }
}
