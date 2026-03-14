using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单汇总金额
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SummaryAmount)]
public record SummaryBillAmountQuery : IAuthorizeableRequest<Result>
{
    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 分组方式 0-按月，1-按天
    /// </summary>
    public int Group { get; set; }
}

public class SummaryBillAmountQueryValidator : AbstractValidator<SummaryBillAmountQuery>
{
    public SummaryBillAmountQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

internal class SummaryBillAmountQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<SummaryBillAmountQuery, Result>
{
    public async Task<Result> Handle(SummaryBillAmountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate.FirstTimeOfDay(), request.EndDate.LastTimeOfDay());

        var bills = await billRepo.Select
            .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date <= end && s.Date >= begin)
            .ToListAsync(cancellationToken);

        var items = new List<BillSummaryAmountWithDateItem>();
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

        Func<Billing, string> groupBy = request.Group == 0 ? b => $"{b.Date.Year}-{b.Date.Month}" : b => $"{b.Date.Year}-{b.Date.Month}-{b.Date.Day}";
        var groups = bills.GroupBy(groupBy);
        foreach (var group in groups)
        {
            var days = GetDays(group.Key);
            var expend = 0M;
            var income = 0M;
            foreach (var bill in bills)
            {
                if (bill.Type == BillType.Expend)
                    expend += bill.Amount;
                else
                    income += bill.Amount;
            }
            items.Add(new BillSummaryAmountWithDateItem(group.Key)
            {
                Expend = expend,
                Income = income,
                ExpendAvg = expend / days,
                IncomeAvg = income / days,
                Surplus = income - expend,
            });

            int GetDays(string key)
            {
                var date = DateTime.Parse(key);
                return request.Group == 0 ? DateTime.DaysInMonth(date.Year, date.Month) : 1;
            }
        }

        return Result.Success(new BillSummaryAmountResult
        {
            Summary = summary,
            Items = items
        });
    }
}