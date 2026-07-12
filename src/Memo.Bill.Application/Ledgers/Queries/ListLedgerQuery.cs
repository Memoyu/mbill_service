using Memo.Bill.Application.Ledgers.Common;
using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Ledgers.Queries;

[Authorize(Permissions = ApiPermission.Ledger.List)]
public record ListLedgerQuery : IAuthorizeableRequest<Result>;

public class CreateAccountCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<User> userRepo
    ) : IRequestHandler<ListLedgerQuery, Result>
{
    public async Task<Result> Handle(ListLedgerQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        // 当前用户加入的所有账单
        var userLedgers = await ledgerUserRepo.Select
            .Include(l => l.Ledger)
            .Where(l => l.UserId == userId)
            .OrderBy(l => l.Sort).ToListAsync(cancellationToken) ?? [];
        var ledgers = userLedgers.Select(l => l.Ledger).ToList();

        var result = new List<LedgerResult>();
        if (ledgers.Count > 0)
        {
            // 账单加入用户数据
            var allLedgerIds = ledgers.Select(l => l.LedgerId).ToList();
            // 获取账单所有加入的成员
            var allLedgerUsers = await ledgerUserRepo.Select.Where(l => allLedgerIds.Contains(l.LedgerId)).ToListAsync(cancellationToken) ?? [];
            var joinUserIds = allLedgerUsers.Select(l => l.UserId).Distinct().ToList();
            var users = await userRepo.Select.Where(u => joinUserIds.Contains(u.UserId)).ToListAsync(cancellationToken);

            // 响应数据组装
            foreach (var ledger in ledgers)
            {
                var dto = mapper.Map<LedgerResult>(ledger);
                var ledgerUsers = allLedgerUsers
                    .Where(l => l.LedgerId == ledger.LedgerId && l.UserId != userId)
                    .Select(l => users.FirstOrDefault(u => u.UserId == l.UserId ))
                    .Where(u => u != null).ToList();
                dto.Users = mapper.Map<List<UserBaseResult>>(ledgerUsers);
                dto.Expend = await billRepo.Select.Where(b => b.LedgerId == ledger.LedgerId && b.Type == BillType.Expend).SumAsync(b => b.Amount, cancellationToken);
                dto.Income = await billRepo.Select.Where(b => b.LedgerId == ledger.LedgerId && b.Type == BillType.Income).SumAsync(b => b.Amount, cancellationToken);
                var userLedger = userLedgers.FirstOrDefault(l => l.LedgerId == ledger.LedgerId) ?? new();
                dto.Sort = userLedger.Sort;
                dto.Color = userLedger.Color;
                result.Add(dto);
            }
        }

        return Result.Success(result);
    }
}