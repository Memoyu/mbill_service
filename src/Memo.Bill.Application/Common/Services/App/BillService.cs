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
    IBaseDefaultRepository<Account> accountRepo
    ) : IBillService
{
    public async Task<BillPageResult<BillPageItemResult>> GetBillPageAsync(PageBillBaseQuery request, CancellationToken cancellationToken = default)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate.FirstTimeOfDay(), request.EndDate.LastTimeOfDay());

        // 排序
        var sort = string.IsNullOrWhiteSpace(request.Sort) ? "date DESC" : request.Sort;

        var bills = await billRepo
            .Select
            .Include(t => t.Category)
            .Include(t => t.Account)
            // .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date >= begin && s.Date <= end)
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .WhereIf(request.CategoryId.HasValue, s => s.CategoryId == request.CategoryId)
            .WhereIf(request.AccountId.HasValue, s => s.AccountId == request.AccountId)
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
            var parAc = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            var parCa = parCas.FirstOrDefault(c => c.CategoryId == b.Category.ParentId);
            b.Category.Name = parCa == null ? b.Category.Name : $"{parCa.Name}-{b.Category.Name}";
            b.Account.Name = parAc == null ? b.Account.Name : $"{parAc.Name}-{b.Account.Name}";
            b.Tags = [.. tags.Where(t => t.BillId == b.BillId).Select(t => mapper.Map<TagBaseResult>(t.Tag))];
            b.RefundAmount = refunds.Where(r => r.BillId == b.BillId).Sum(r => r.Amount);
        });

        return new BillPageResult<BillPageItemResult>(dtos, total);
    }
}
