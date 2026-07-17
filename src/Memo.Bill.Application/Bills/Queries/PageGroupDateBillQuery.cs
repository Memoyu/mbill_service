using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单分页日期分组
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.PageGroupDate)]
public record PageGroupDateBillQuery : PageBillBaseQuery, IAuthorizeableRequest<Result>;

public class PageGroupDateBillQueryValidator : AbstractValidator<PageGroupDateBillQuery>
{
    public PageGroupDateBillQueryValidator()
    {
        RuleFor(x => x.LedgerIds)
           .NotEmpty()
           .WithMessage("账本Id不能为空");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate)
            .WithMessage("结束时间必须晚于开始时间");
    }
}

internal class PageGroupDateBillQueryHandler(
    IBaseDefaultRepository<Billing> billRepo,
    IBillService billService
    ) : IRequestHandler<PageGroupDateBillQuery, Result>
{
    public async Task<Result> Handle(PageGroupDateBillQuery request, CancellationToken cancellationToken)
    {
        var result = await billService.GetBillPageAsync(request, cancellationToken);

        var groupRes = new List<PageGroupDateBillResult>();

        var dateGroup = result.Items.GroupBy(b => b.Date.Date).ToList();

        foreach (var gm in dateGroup)
        {
            var date = gm.Key;
            var dateEnd = date.AddDays(1).AddSeconds(-1);
            var allBill = await billRepo.Select.Where(b => b.Date >= date && b.Date <= dateEnd).ToListAsync(b => new { b.Type, b.Amount }, cancellationToken) ?? [];
            groupRes.Add(new PageGroupDateBillResult
            {
                Date = date,
                Expend = allBill.Where(b => b.Type == BillType.Expend).Sum(b => b.Amount),
                Income = allBill.Where(b => b.Type == BillType.Income).Sum(b => b.Amount),
                Items = [.. result.Items.Where(b => b.Date >= date && b.Date <= dateEnd)]
            });
        }

        return Result.Success(new BillPageResult<PageGroupDateBillResult>(groupRes, result.Total));
    }
}
