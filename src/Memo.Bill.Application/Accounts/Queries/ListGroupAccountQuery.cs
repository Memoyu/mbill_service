using DnsClient.Protocol;
using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Accounts.Queries;

[Authorize(Permissions = ApiPermission.Account.ListGroup)]
public record ListGroupAccountQuery() : IAuthorizeableRequest<Result>;

public class ListGroupAccountQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<FrequencyRecord> billPropFreqRepo
    ) : IRequestHandler<ListGroupAccountQuery, Result>
{
    public async Task<Result> Handle(ListGroupAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await accountRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => new { x.Sort, x.CreateTime }).ToListAsync(cancellationToken) ?? [];

        var dto = new AccountGroupResult();
        if (entities.Count > 0)
        {
            var ids = new List<long>();
            var parents = new List<AccountWithChildResult>();
            var childs = new List<AccountResult>();
            foreach (var ac in entities)
            {
                if (!ac.ParentId.HasValue)
                    parents.Add(mapper.Map<AccountWithChildResult>(ac));
                else
                    childs.Add(mapper.Map<AccountResult>(ac));
                ids.Add(ac.AccountId);
            }
            parents.ForEach(d => { d.Childs = [.. childs.Where(x => x.ParentId == d.AccountId)]; });

            // 构建常用集合
            var records = await billPropFreqRepo.Select.Where(r => r.Type == FrequencyRecordType.Account && ids.Contains(r.RecordId)).ToListAsync(cancellationToken) ?? [];
            var tops = entities.Where(x => x.Top).Take(10).Select(mapper.Map<AccountWithChildResult>).ToList();
            var count = 10 - tops.Count;
            if (count > 0)
            {
                var rs = records.Where(r => !tops.Any(e => e.AccountId == r.RecordId))
                     .OrderByDescending((r) => r.Frequency)
                     .Take(count)
                     .Select(r => mapper.Map<AccountWithChildResult>(entities.First(e => r.RecordId == e.AccountId)))
                     .ToList();
                tops.AddRange(rs);
            }

            dto = new AccountGroupResult { Tops = tops, List = parents };
        }
        return Result.Success(dto);
    }
}
