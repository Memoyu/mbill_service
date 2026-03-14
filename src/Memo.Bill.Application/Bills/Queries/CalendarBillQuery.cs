using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单日历
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.Calendar)]
public record CalendarBillQuery(DateTime BeginDate, DateTime EndDate) : IAuthorizeableRequest<Result>;

public class CalendarBillQueryValidator : AbstractValidator<CalendarBillQuery>
{
    public CalendarBillQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

internal class CalendarBillQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<CalendarBillQuery, Result>
{
    public async Task<Result> Handle(CalendarBillQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var begin = request.BeginDate.FirstDayOfMonth();
        var end = request.EndDate.LastDayOfMonth().LastTimeOfDay();

        var bills = await billRepo.Select
            .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date <= end && s.Date >= begin)
            .ToListAsync(cancellationToken);

        var res = bills.GroupBy(b => b.Date.Date).Select(g => new BillCalendarItem
        {
            Date = g.Key,
            Count = g.Count(),
        }).ToList();

        return Result.Success(res);
    }
}