using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单分页日期分组
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.PageGroupDate)]
public record PageBillGroupByDateQuery : PageBillBaseQuery, IAuthorizeableRequest<Result>;

public class PageBillGroupByDateQueryValidator : AbstractValidator<PageBillGroupByDateQuery>
{
    public PageBillGroupByDateQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

internal class PageBillGroupByDateQueryHandler(
    IBillService billService
    ) : IRequestHandler<PageBillGroupByDateQuery, Result>
{
    public async Task<Result> Handle(PageBillGroupByDateQuery request, CancellationToken cancellationToken)
    {
        var result = await billService.GetBillPageAsync(request, cancellationToken);

        var groupRes = new List<BillPageGroupByMonth>();
        var groupByMonth = result.Items.GroupBy(b => new { b.Date.Year, b.Date.Month }).ToList();
        foreach (var gm in groupByMonth)
        {
            var year = gm.Key.Year;
            var month = gm.Key.Month;
            groupRes.Add(new BillPageGroupByMonth 
            {
                Month = $"{year}-{month}",
                Items = [.. result.Items.Where(b => b.Date.Year == year && b.Date.Month == month)]
            });
        }

        return Result.Success(new BillPageResult<BillPageGroupByMonth>(groupRes, result.Total)
        {
            Date = result.Date,
            Expend = result.Expend,
            Income = result.Income,
        });
    }
}
