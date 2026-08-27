using Memo.Bill.Application.Accounts.Common;

namespace Memo.Bill.Application.Accounts.Queries;

[Authorize(Permissions = ApiPermission.Account.ListTop)]
public record ListTopAccountQuery() : IAuthorizeableRequest<Result>;

public class ListTopAccountQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<FrequencyRecord> frequencyRecordRepo
    ) : IRequestHandler<ListTopAccountQuery, Result>
{
    public async Task<Result> Handle(ListTopAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var topAccounts = await accountRepo.Select.Where(x => x.CreateUserId == userId && x.Top).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var tops = new List<AccountResult>();
        var ids = new List<long>();
        foreach (var ac in topAccounts)
        {
            if (ac.Top && tops.Count < 10)
            {
                tops.Add(mapper.Map<AccountResult>(ac));
                ids.Add(ac.AccountId);
            }
        }

        // 设置的常用项不足10个时，根据使用记录筛出常用项
        if (tops.Count < 10)
        {
            var count = 10 - tops.Count;
            var records = await frequencyRecordRepo.Select.From<Account>((r, ac) => r.LeftJoin(r => r.RecordId == ac.AccountId))
                .Where((r, ac) => r.Type == FrequencyRecordType.Account && ac.CreateUserId == userId)
                .WhereIf(ids.Count > 0, (r, ac) => !ids.Contains(r.RecordId))
                .OrderByDescending((r, ac) => r.Frequency)
                .Limit(count)
                .ToListAsync((r, ac) => ac, cancellationToken);
            tops.AddRange(mapper.Map<List<AccountResult>>(records));
        }

        return Result.Success(tops);
    }
}
