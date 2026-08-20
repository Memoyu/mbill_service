using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

internal record BillAmountSummaryDto(BillType Type, decimal Amount, DateTime Date);

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
            .Where(s => s.CreateUserId == userId) // 统计时，只统计个人的
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .Where(s => s.Date <= end && s.Date >= begin)
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .ToListAsync(b => new BillAmountSummaryDto(b.Type, b.Amount, b.Date), cancellationToken);

        var summary = GetSummary(bills, end.Subtract(begin).Days);

        // 时间范围内分组汇总
        var series = new List<BillSummaryAmountItem>();
        if (request.Series > 0)
        {
            var dates = request.Series == 1 ? begin.GetMonthRanges(end) : begin.GetDateRanges(end);
            foreach (var date in dates)
            {
                // 当前天数
                var days = request.Series == 1 ? DateTime.DaysInMonth(date.Year, date.Month) : 1;
                var dateBills = bills
                    .Where(b => request.Series == 1 ? (b.Date.Year == date.Date.Year && b.Date.Month == date.Date.Month) : b.Date.Date == date.Date)
                    .ToList();

                var sm = GetSummary(dateBills, days);
                sm.Date = request.Series == 1 ? date.ToString("yyyy-MM") : date.ToString("yyyy-MM-dd");
                series.Add(sm);
            }
        }

        result.Summary = summary;
        result.Series = series;
        return Result.Success(result);
    }

    private BillSummaryAmountItem GetSummary(List<BillAmountSummaryDto> bills, int days)
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

        return new BillSummaryAmountItem
        {
            Expend = expend,
            Income = income,
            ExpendAvg = expend / days,
            IncomeAvg = income / days,
            Surplus = income - expend,
            ExpendHighest = expendHighest,
            ExpendLowst = expendLowst,
            IncomeHighest = incomeHighest,
            IncomeLowst = incomeLowst,
        };
    }
}