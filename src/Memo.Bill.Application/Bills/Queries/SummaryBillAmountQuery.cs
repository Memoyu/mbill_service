using Memo.Bill.Application.Bills.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 账单金额汇总
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SummaryAmount)]
public record SummaryBillAmountQuery(
    int Series // 分组汇总: 0：不分组，1：按月，2：按日
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
        var (begin, end) = (request.BeginDate!.Value.FirstTimeOfDay(), request.EndDate!.Value.LastTimeOfDay());

        var result = new BillSummaryAmountResult();
        // 账本为空，则不需要继续进行查询
        request.LedgerIds = await billService.FilterLedgerAsync(request.LedgerIds, cancellationToken);
        if (request.LedgerIds.Count < 1)
            return Result.Success(result);

        var bills = await billRepo.Select
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .Where(s => s.Date <= end && s.Date >= begin)
            .ToListAsync(cancellationToken);

        // 时间范围内汇总
        var totalExpend = 0M;
        var totalIncome = 0M;
        foreach (var bill in bills)
        {
            if (bill.Type == BillType.Expend)
                totalExpend += bill.Amount;
            else
                totalIncome += bill.Amount;
        }
        var totalDays = end.Subtract(begin).Days;
        var summary = new BillSummaryAmountItem
        {
            Expend = totalExpend,
            Income = totalIncome,
            ExpendAvg = totalExpend / totalDays,
            IncomeAvg = totalIncome / totalDays,
            Surplus = totalIncome - totalExpend,
        };

        // 时间范围内分组汇总
        var series = new List<BillSummaryAmountWithDateItem>();
        if (request.Series > 0)
        {
            var dates = request.Series == 1 ?  begin.GetMonthRanges(end) : begin.GetDateRanges(end);
            foreach (var date in dates)
            {
                // 当前天数
                var days = request.Series == 1 ? DateTime.DaysInMonth(date.Year, date.Month) : 1;
                var dateBills = bills
                    .Where(b => request.Series == 1 ?  (b.Date.Year == date.Date.Year && b.Date.Month == date.Date.Month) : b.Date.Date == date.Date)
                    .ToList();
                var expend = 0M;
                var income = 0M;
                foreach (var bill in dateBills)
                {
                    if (bill.Type == BillType.Expend)
                        expend += bill.Amount;
                    else
                        income += bill.Amount;
                }

                var d = request.Series == 1 ? date.ToString("yyyy-MM") : date.ToString("yyyy-MM-dd");
                series.Add(new BillSummaryAmountWithDateItem(d)
                {
                    Expend = expend,
                    Income = income,
                    ExpendAvg = expend / days,
                    IncomeAvg = income / days,
                    Surplus = income - expend,
                });
            }
        }

        result.Summary = summary;
        result.Series = series;
        return Result.Success(result);
    }
}