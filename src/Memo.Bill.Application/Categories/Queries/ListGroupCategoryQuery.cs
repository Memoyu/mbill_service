using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Queries;

[Authorize(Permissions = ApiPermission.Category.ListGroup)]
public record ListGroupCategoryQuery() : IAuthorizeableRequest<Result>;

public class ListGroupCategoryQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<BillPropUsageRecord> billPropUsageRepo
    ) : IRequestHandler<ListGroupCategoryQuery, Result>
{
    public async Task<Result> Handle(ListGroupCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await categoryRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var dto = new CategoryGroupResult();
        if (entities.Count > 0)
        {
            var ids = entities.Select(e => e.CategoryId).ToList();
            var expendTops = new List<CategoryResult>();
            var incomeTops = new List<CategoryResult>();
            var parents = new List<CategoryGroupItem>();
            var childs = new List<CategoryResult>();
            foreach (var ca in entities)
            {
                if (ca.Top)
                {
                    if (ca.Type == BillType.Expend && expendTops.Count < 10)
                        expendTops.Add(mapper.Map<CategoryResult>(ca));
                    if (ca.Type == BillType.Income && expendTops.Count < 10)
                        incomeTops.Add(mapper.Map<CategoryResult>(ca));
                }

                if (!ca.ParentId.HasValue)
                    parents.Add(mapper.Map<CategoryGroupItem>(ca));
                else
                    childs.Add(mapper.Map<CategoryResult>(ca));
            }

            parents.ForEach(d =>
            {
                d.Childs = [.. childs.Where(x => x.ParentId == d.CategoryId)];
            });

            // 设置的常用项不足10个时，根据使用记录筛出常用项
            if (expendTops.Count < 10)
            {
                // 还缺少多少个
                var count = 10 - expendTops.Count;
                var records = await billPropUsageRepo.Select
                    .Where(r => r.RecordType == BillPropUsageRecordType.Category && r.Type == BillType.Expend && ids.Contains(r.RecordId))
                    .OrderByDescending(r => r.Frequency)
                    .Limit(count)
                    .ToListAsync(cancellationToken);
                expendTops.AddRange(mapper.Map<List<CategoryResult>>(entities.Where(e => records.Any(r => r.RecordId == e.CategoryId))));
            }
            if (incomeTops.Count < 10)
            {
                // 还缺少多少个
                var count = 10 - incomeTops.Count;
                var records = await billPropUsageRepo.Select
                    .Where(r => r.RecordType == BillPropUsageRecordType.Category && r.Type == BillType.Income && ids.Contains(r.RecordId))
                    .OrderByDescending(r => r.Frequency)
                    .Limit(count)
                    .ToListAsync(cancellationToken);
                incomeTops.AddRange(mapper.Map<List<CategoryResult>>(entities.Where(e => records.Any(r => r.RecordId == e.CategoryId))));
            }

            dto.ExpendTops = expendTops;
            dto.IncomeTops = incomeTops;
            dto.Expends = [.. parents.Where(p => p.Type == BillType.Expend)];
            dto.Incomes = [.. parents.Where(p => p.Type == BillType.Income)];
        }
        return Result.Success(dto);
    }
}
