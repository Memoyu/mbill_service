using Memo.Bill.Application.Bills.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 账单金额汇总
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SummaryAmount)]
public record SummaryBillAmountQuery(
    int Series // 分组汇总: 0：不分组，1：按月，2：按日，3：按月、日
) : BillQueryRequest, IAuthorizeableRequest<Result>;

public class SummaryBillAmountQueryValidator : AbstractValidator<SummaryBillAmountQuery>
{
    public SummaryBillAmountQueryValidator()
    {
        RuleFor(x => x.BeginDate)
            .NotEmpty()
            .WithMessage("开始时间不能为空");

        RuleFor(x => x.EndDate)
             .NotEmpty()
            .WithMessage("结束时间不能为空");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

internal class SummaryBillAmountQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBillService billService,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<SummaryBillAmountQuery, Result>
{
    public async Task<Result> Handle(SummaryBillAmountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate!.Value.StartOfDay(), request.EndDate!.Value.EndOfDay());

        var result = new BillSummaryAmountResult();
        // 账本为空，则不需要继续进行查询
        request.LedgerIds = await billService.FilterLedgerAsync(request.LedgerIds, cancellationToken);
        if (request.LedgerIds.Count < 1)
            return Result.Success(result);

        var bills = await billRepo.Select
            .Where(s => s.CreateUserId == userId) // 统计时，只统计个人的
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .Where(s => s.Date <= end && s.Date >= begin)
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .ToListAsync(b => new BillAmountSummaryDto(b.BillId, b.Type, b.Amount, b.Date), cancellationToken);

        var summary = GetSummary(bills, end.Subtract(begin).Days, request.Series, begin);

        // 时间范围内分组汇总
        var series = new List<BillSummaryAmountResult>();
        if (request.Series > 0)
        {
            if (request.Series == 3)
            {
                var dates = begin.GetRanges(end);
                // 按月分组
                var mgs = dates.GroupBy(d => new { d.Year, d.Month }).ToList();
                foreach (var g in mgs)
                {
                    var mg = g.Key;
                    var m = DateTime.Parse($"{mg.Year}-{mg.Month}-01");
                    // 当前天数
                    var days = DateTime.DaysInMonth(mg.Year, mg.Month);
                    var mBills = bills.Where(b => b.Date >= m.StartOfMonth() && b.Date <= m.EndOfMonth()).ToList();
                    var res = new BillSummaryAmountResult { Summary = GetSummary(mBills, days, 2, m) };
                    foreach (var d in g)
                    {
                        var dBills = mBills.Where(b => b.Date >= d.StartOfDay() && b.Date <= d.EndOfDay()).ToList();
                        res.Items.Add(new BillSummaryAmountResult { Summary = GetSummary(dBills, 1, 1, d) });
                    }
                    series.Add(res);
                }
            }
            else
            {
                var dates = request.Series == 1 ? begin.GetRanges(end, 1) : begin.GetRanges(end);
                foreach (var date in dates)
                {
                    // 当前天数
                    var days = request.Series == 1 ? DateTime.DaysInMonth(date.Year, date.Month) : 1;
                    var dateBills = bills
                        .Where(b => request.Series == 1 ? (b.Date.Year == date.Date.Year && b.Date.Month == date.Date.Month) : b.Date.Date == date.Date)
                        .ToList();

                    series.Add(new BillSummaryAmountResult { Summary = GetSummary(dateBills, days, request.Series, date) });
                }
            }
        }

        result.Summary = summary;
        result.Items = series;
        return Result.Success(result);
    }

    private BillSummaryAmountItem GetSummary(List<BillAmountSummaryDto> bills, int days, int series, DateTime date)
    {
        // 时间范围内汇总
        var expend = 0M;
        var income = 0M;
        var expendHighest = 0M;
        var expendLowst = 0M;
        var incomeHighest = 0M;
        var incomeLowst = 0M;
        foreach (var bill in bills)
        {
            var amount = bill.Amount;
            if (bill.Type == BillType.Expend)
            {
                expendHighest = Math.Max(amount, expendHighest);
                expendLowst = Math.Min(amount, expendHighest);
                expend += amount;
            }
            else
            {
                incomeHighest = Math.Max(amount, incomeHighest);
                incomeLowst = Math.Min(amount, incomeLowst);
                income += amount;
            }
        }

        // 查一天时，days为0
        days = days == 0 ? 1 : days;
        var expendAvg = expend / days;
        var incomeAvg = income / days;
        
        return new BillSummaryAmountItem
        {
            Date = series == 1 ? date.ToString("yyyy-MM") : date.ToString("yyyy-MM-dd"),
            Expend = expend,
            Income = income,
            Surplus = income - expend,
            ExpendAvg = expendAvg,
            IncomeAvg = incomeAvg,
            SurplusAvg = incomeAvg - expendAvg,
            ExpendHighest = expendHighest,
            ExpendLowst = expendLowst,
            IncomeHighest = incomeHighest,
            IncomeLowst = incomeLowst,
        };
    }
}