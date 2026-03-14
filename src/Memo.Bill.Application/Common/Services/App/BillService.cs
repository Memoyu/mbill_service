using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Bills.Queries;

namespace Memo.Bill.Application.Common.Services.App;

[AppService]
internal class BillService(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo
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
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date >= begin && s.Date <= end)
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .WhereIf(request.CategoryId.HasValue, s => s.CategoryId == request.CategoryId)
            .WhereIf(request.AccountId.HasValue, s => s.AccountId == request.AccountId)
            .OrderBy(sort)
            .ToPageListAsync(request, out var total, cancellationToken);

        var expend = 0m;
        var income = 0m;
        foreach (var bill in bills)
        {
            if (bill.Type == BillType.Expend)
                expend += bill.Amount;
            else
                income += bill.Amount;
        }

        return new BillPageResult<BillResult>(mapper.Map<List<BillResult>>(bills), total)
        {
            Date = begin,
            Expend = expend,
            Income = income,
        };
    }
}
