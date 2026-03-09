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

        var entities = await accountRepo.Select.Where(c => c.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<AccountGroupResult>>(entities.Where(c => !c.ParentId.HasValue).OrderByDescending(d => d.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<AccountResult>>(entities.Where(d => d.ParentId == d.AccountId).OrderByDescending(d => d.Sort));
        });
        return Result.Success(dtos);
    }
}
