using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Bills.Queries;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Common.Services.App;

[AppService]
internal class BillService(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo
    ) : IBillService
{
    public async Task<BillPageResult> GetBillPageAsync(PageBillBaseQuery request, CancellationToken cancellationToken = default)
    {
        var userId = currentUserProvider.UserId;

        var begin = request.Date.Date;
        var end = begin.AddDays(1).AddSeconds(-1);

        //当月
        if (request.DateType == 1)
        {
            begin = DateTime.Parse($"{request.Date.Year}-{request.Date.Month}-01");
            end = begin.AddDays(1 - begin.Day).Date.AddMonths(1).AddSeconds(-1);
        }
        // 当年
        else if (request.DateType == 2)
        {
            begin = DateTime.Parse($"{request.Date.Year}-01-01");
            end = begin.AddYears(1).AddSeconds(-1);
        }

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

        return new BillPageResult(mapper.Map<List<BillResult>>(bills), total)
        {
            Date = begin,
            Expend = expend.FormatAmount(),
            Income = income.FormatAmount(),
        };
    }
}
