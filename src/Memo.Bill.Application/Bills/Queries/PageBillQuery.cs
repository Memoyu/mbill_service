using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单分页
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.Page)]
public record PageBillQuery : BillQueryRequest, IAuthorizeableRequest<Result>;

public class PageBillQueryValidator : AbstractValidator<PageBillQuery>
{
    public PageBillQueryValidator()
    {
        RuleFor(x => x.LedgerIds)
           .NotEmpty()
           .WithMessage("账本Id不能为空");
    }
}

internal class PageBillQueryHandler(
    IBaseDefaultRepository<Billing> billRepo,
    IBillService billService
    ) : IRequestHandler<PageBillQuery, Result>
{
    public async Task<Result> Handle(PageBillQuery request, CancellationToken cancellationToken)
    {
        var result = await billService.PageAsync(request, cancellationToken);
        var groupRes = new List<BillGroupDatePageResult>();
        var dateGroup = result.Items.GroupBy(b => b.Date.Date).ToList();

        foreach (var gm in dateGroup)
        {
            var date = gm.Key;
            var dateEnd = date.AddDays(1).AddSeconds(-1);
            var dateBills = await billRepo.Select
                .Where(s => request.LedgerIds!.Contains(s.LedgerId))
                .Where(b => b.Date >= date && b.Date <= dateEnd)
                .ToListAsync(b => new { b.Type, b.Amount }, cancellationToken) ?? [];
            groupRes.Add(new BillGroupDatePageResult
            {
                Date = date,
                Expend = dateBills.Where(b => b.Type == BillType.Expend).Sum(b => b.Amount),
                Income = dateBills.Where(b => b.Type == BillType.Income).Sum(b => b.Amount),
                Items = [.. result.Items.Where(b => b.Date >= date && b.Date <= dateEnd)]
            });
        }

        return Result.Success(new PaginationResult<BillGroupDatePageResult>(groupRes, result.Total));
    }
}
