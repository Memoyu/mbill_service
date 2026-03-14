using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单汇总分类
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SummaryCategory)]
public record SummaryBillCategoryQuery : IAuthorizeableRequest<Result>
{
    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime EndDate { get; set; }
}

public class SummaryBillCategoryQueryValidator : AbstractValidator<SummaryBillCategoryQuery>
{
    public SummaryBillCategoryQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

internal class SummaryBillCategoryQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<SummaryBillCategoryQuery, Result>
{
    public async Task<Result> Handle(SummaryBillCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate.FirstTimeOfDay(), request.EndDate.LastTimeOfDay());

        var bills = await billRepo.Select
            .Include(s => s.Category)
            .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date <= end && s.Date >= begin)
            .ToListAsync(cancellationToken);

        var res = new List<BillSummaryCategoryItem>();
        // 父分类
        var parentCategoryIds = bills.Where(b => b.Category.ParentId.HasValue).Select(b => b.Category.ParentId).ToList();
        var parentCategories = await categoryRepo.Select.Where(c => parentCategoryIds.Contains(c.CategoryId)).ToListAsync(cancellationToken);
        // 子类分组
        var childGroups = bills.Where(b => b.Category.ParentId.HasValue).GroupBy(b => new { b.CategoryId }).Select(g =>
        {
            var c = g.First().Category;
            var item = new BillSummaryCategoryItem
            {
                Category = mapper.Map<CategoryBaseResult>(c),
                Amount = g.Sum(b => b.Amount)
            };
            return new { c.ParentId, Item = item };
        });

        foreach (var category in parentCategories)
        {
            var c = mapper.Map<CategoryBaseResult>(category);
            var childs = childGroups.Where(gc => gc.ParentId == c.CategoryId).Select(gc => gc.Item).ToList();
            res.Add(new BillSummaryCategoryItem
            {
                Category = c,
                Amount = childs.Sum(b => b.Amount),
                Childs = childs
            });
        }

        return Result.Success(res);
    }
}