using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Bills.Queries;
using Memo.Bill.Application.Categories.Common;
using Memo.Bill.Application.Tags.Common;

namespace Memo.Bill.Application.Common.Services.App;

[AppService]
internal class BillService(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<BillTag> billTagRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IBillService
{
    public async Task<BillPageResult<BillResult>> GetBillPageAsync(PageBillBaseQuery request, CancellationToken cancellationToken = default)
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


        var parCaIds = bills.Select(b => b.Category.ParentId).Where(p => p.HasValue).Distinct().ToList();
        var parCas = await categoryRepo.Select
            .Where(t => parCaIds.Contains(t.CategoryId))
            .ToListAsync(cancellationToken);

        var parAcIds = bills.Select(b => b.Account.ParentId).Where(p => p.HasValue).Distinct().ToList();
        var parAcs = await accountRepo.Select
            .Where(t => parAcIds.Contains(t.AccountId))
            .ToListAsync(cancellationToken);

        var billIds = bills.Select(b => b.BillId).ToList();
        var tags = await billTagRepo.Select
            .Include(t => t.Tag)
            .Where(t => billIds.Contains(t.BillId))
            .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<BillResult>>(bills);
        dtos.ForEach(b =>
        {
            var parAc = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            var parCa = parCas.FirstOrDefault(c => c.CategoryId == b.Category.ParentId);
            b.Category.Name = parCa == null ? b.Category.Name : $"{parCa.Name}-{b.Category.Name}";
            b.Account.Name = parAc == null ? b.Account.Name : $"{parAc.Name}-{b.Account.Name}";
            b.Tags = [.. tags.Where(t => t.BillId == b.BillId).Select(t => mapper.Map<TagBaseResult>(t.Tag))];
        });

        return new BillPageResult<BillResult>(dtos, total);
    }
}
