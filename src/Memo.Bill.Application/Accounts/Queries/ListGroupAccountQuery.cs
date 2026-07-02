using Memo.Bill.Application.Accounts.Common;

namespace Memo.Bill.Application.Accounts.Queries;

[Authorize(Permissions = ApiPermission.Account.ListGroup)]
public record ListGroupAccountQuery() : IAuthorizeableRequest<Result>;

public class ListGroupAccountQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<ListGroupAccountQuery, Result>
{
    public async Task<Result> Handle(ListGroupAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entities = await accountRepo.Select.Where(x => x.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<AccountGroupResult>>(entities.Where(x => !x.ParentId.HasValue).OrderBy(x => x.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<AccountResult>>(entities.Where(x => x.ParentId == d.AccountId).OrderBy(x => x.Sort));
        });
        return Result.Success(dtos);
    }
}
