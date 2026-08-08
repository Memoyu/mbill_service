using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Bills.Queries;
using Memo.Bill.Application.Tags.Common;

namespace Memo.Bill.Application.Common.Services.App;

[AppService]
internal class BillService(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<BillTag> billTagRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo
    ) : IBillService
{
    public async Task<BillPageResult<BillPageItemResult>> GetBillPageAsync(PageBillBaseQuery request, CancellationToken cancellationToken = default)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate.FirstTimeOfDay(), request.EndDate.LastTimeOfDay());

        // 账本
        var ledgerIds = request.LedgerIds ?? [];
        var userLedgerIds = await ledgerUserRepo.Select
            .Where(l => l.UserId == userId)
            .WhereIf(ledgerIds.Count > 0, l => ledgerIds.Contains(l.LedgerId))
            .ToListAsync(l => l.LedgerId, cancellationToken);

        // 标签
        var tagBillIds = new List<long>();
        var tagIds = request.TagIds ?? [];
        if (tagIds.Count > 0)
        {
            tagBillIds = await billTagRepo.Select
                .Where(t => tagIds.Contains(t.TagId)).ToListAsync(t => t.BillId, cancellationToken);
        }

        // 排序
        var sort = string.IsNullOrWhiteSpace(request.Sort) ? "date DESC" : request.Sort;
        var bills = await billRepo
            .Select
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(s => s.Date >= begin && s.Date <= end)
            .Where(s => userLedgerIds.Contains(s.LedgerId))
            .WhereIf(tagBillIds.Count > 0, s => tagBillIds.Contains(s.BillId))
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .WhereIf((request.CategoryIds ?? []).Count > 0, s => request.CategoryIds!.Contains(s.CategoryId))
            .WhereIf((request.AccountIds ?? []).Count > 0, s => request.AccountIds!.Contains(s.AccountId))
            .OrderBy(sort)
            .ToPageListAsync(request, out var total, cancellationToken);

        var billIds = new HashSet<long>();
        var parCaIds = new HashSet<long>();
        var parAcIds = new HashSet<long>();

        foreach (var rb in bills)
        {
            billIds.Add(rb.BillId);

            if (rb.Category.ParentId.HasValue)
                parCaIds.Add(rb.Category.ParentId.Value);
            if (rb.Account.ParentId.HasValue)
                parAcIds.Add(rb.Account.ParentId.Value);
        }

        var parCas = await categoryRepo.Select.Where(t => parCaIds.Contains(t.CategoryId)).ToListAsync(cancellationToken);
        var parAcs = await accountRepo.Select.Where(t => parAcIds.Contains(t.AccountId)).ToListAsync(cancellationToken);
        var tags = await billTagRepo.Select.Include(t => t.Tag).Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);
        var refunds = await billRefundRepo.Select.Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<BillPageItemResult>>(bills);
        dtos.ForEach(b =>
        {
            b.Category.Parent = parCas.FirstOrDefault(c => c.CategoryId == b.Category.ParentId);
            b.Account.Parent = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            b.Tags = [.. tags.Where(t => t.BillId == b.BillId).Select(t => mapper.Map<TagBaseResult>(t.Tag))];
            b.RefundAmount = refunds.Where(r => r.BillId == b.BillId).Sum(r => r.Amount);
        });

        return new BillPageResult<BillPageItemResult>(dtos, total);
    }
}
