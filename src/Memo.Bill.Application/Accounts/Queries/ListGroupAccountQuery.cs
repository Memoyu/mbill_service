using Memo.Bill.Application.Accounts.Common;

namespace Memo.Bill.Application.Accounts.Queries;

[Authorize(Permissions = ApiPermission.Account.ListGroup)]
public record ListGroupAccountQuery() : IAuthorizeableRequest<Result>;

public class ListGroupAccountQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<BillPropUsageRecord> billPropUsageRepo
    ) : IRequestHandler<ListGroupAccountQuery, Result>
{
    public async Task<Result> Handle(ListGroupAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await accountRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var dto = new AccountGroupResult();
        if (entities.Count > 0)
        {
            var ids = entities.Select(e => e.AccountId).ToList();
            var tops = new List<AccountResult>();
            var parents = new List<AccountGroupItem>();
            var childs = new List<AccountResult>();
            foreach (var ac in entities)
            {
                if (ac.Top && tops.Count < 10)
                    tops.Add(mapper.Map<AccountResult>(ac));

                if (!ac.ParentId.HasValue)
                    parents.Add(mapper.Map<AccountGroupItem>(ac));
                else
                    childs.Add(mapper.Map<AccountResult>(ac));
            }

            parents.ForEach(d =>
            {
                d.Childs = [.. childs.Where(x => x.ParentId == d.AccountId)];
            });

            // 设置的常用项不足10个时，根据使用记录筛出常用项
            if (tops.Count < 10)
            {
                // 还缺少多少个
                var count = 10 - tops.Count;
                var records = await billPropUsageRepo.Select
                    .Where(r => r.RecordType == BillPropUsageRecordType.Account && ids.Contains(r.RecordId))
                    .OrderByDescending(r => r.Frequency)
                    .Limit(count)
                    .ToListAsync(cancellationToken);
                tops.AddRange(mapper.Map<List<AccountResult>>(entities.Where(e => records.Any(r => r.RecordId == e.AccountId))));
            }
           

            dto.Tops = tops;
            dto.Items = parents;
        }
        return Result.Success(dto);
    }
}
