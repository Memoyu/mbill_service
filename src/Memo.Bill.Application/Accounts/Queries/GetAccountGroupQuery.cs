using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Accounts.Queries;

[Authorize(Permissions = ApiPermission.Account.GetGroup)]
public record GetAccountGroupQuery() : IAuthorizeableRequest<Result>;

public class GetAccountGroupQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<GetAccountGroupQuery, Result>
{
    public async Task<Result> Handle(GetAccountGroupQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entities = await accountRepo.Select.Where(x => x.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<AccountGroupResult>>(entities.Where(x => !x.ParentId.HasValue).OrderByDescending(x => x.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<AccountResult>>(entities.Where(x => x.ParentId == d.AccountId).OrderByDescending(x => x.Sort));
        });
        return Result.Success(dtos);
    }
}
