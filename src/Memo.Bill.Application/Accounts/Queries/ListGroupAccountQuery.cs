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
        var entities = await accountRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var dto = new AccountGroupResult();
        if (entities.Count > 0)
        {
            var parents = new List<AccountGroupItem>();
            var childs = new List<AccountResult>();
            foreach (var ac in entities)
            {
                if (!ac.ParentId.HasValue)
                    parents.Add(mapper.Map<AccountGroupItem>(ac));
                else
                    childs.Add(mapper.Map<AccountResult>(ac));
            }

            parents.ForEach(d =>
            {
                d.Childs = [.. childs.Where(x => x.ParentId == d.AccountId)];
            });

            dto.Items = parents;
        }
        return Result.Success(dto);
    }
}
