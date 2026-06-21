using Memo.Bill.Application.Ledgers.Common;
using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Ledgers.Queries;

[Authorize(Permissions = ApiPermission.Ledger.List)]
public record ListLedgerQuery : IAuthorizeableRequest<Result>;

public class CreateAccountCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<ListLedgerQuery, Result>
{
    public async Task<Result> Handle(ListLedgerQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await ledgerRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var result = new List<LedgerResult>();
        if (entities.Count > 0)
        {
            var ledgerIds = entities.Select(l => l.LedgerId).ToList();
            var ledgerUsers = await ledgerUserRepo.Select.Include(l => l.User).Where(l => ledgerIds.Contains(l.LedgerId)).ToListAsync(cancellationToken);

            foreach (var e in entities)
            {
                var dto = mapper.Map<LedgerResult>(e);
                dto.Users = mapper.Map<List<UserBaseResult>>(ledgerUsers.Where(lu => lu.LedgerId == e.LedgerId).Select(lu => lu.User).ToList());

                dto.Expend = await billRepo.Select.Where(b => b.LedgerId == e.LedgerId && b.Type == BillType.Expend ).SumAsync(b => b.Amount, cancellationToken);
                dto.Income = await billRepo.Select.Where(b => b.LedgerId == e.LedgerId && b.Type == BillType.Income).SumAsync(b => b.Amount, cancellationToken);

                result.Add(dto);
            }
        }

        return Result.Success(result);
    }
}