using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Queries;

[Authorize(Permissions = ApiPermission.Category.ListGroup)]
public record ListGroupCategoryQuery() : IAuthorizeableRequest<Result>;

public class ListGroupCategoryQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<FrequencyRecord> billPropFreqRepo
    ) : IRequestHandler<ListGroupCategoryQuery, Result>
{
    public async Task<Result> Handle(ListGroupCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await categoryRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => new { x.Sort, x.CreateTime }).ToListAsync(cancellationToken) ?? [];

        var dto = new CategoryGroupsResult();
        if (entities.Count > 0)
        {
            var ids = new List<long>();
            var parents = new List<CategoryWithChildsResult>();
            var childs = new List<CategoryResult>();
            foreach (var ca in entities)
            {
                if (!ca.ParentId.HasValue)
                    parents.Add(mapper.Map<CategoryWithChildsResult>(ca));
                else
                    childs.Add(mapper.Map<CategoryResult>(ca));
                ids.Add(ca.CategoryId);
            }
            parents.ForEach(d => { d.Childs = [.. childs.Where(x => x.ParentId == d.CategoryId)]; });

            var records = await billPropFreqRepo.Select.Where(r => r.Type == FrequencyRecordType.Category && ids.Contains(r.RecordId)).ToListAsync(cancellationToken) ?? [];
            dto.Expends = new CategoryGroupItem { Tops = GetTops(records, BillType.Expend), List = [.. parents.Where(p => p.Type == BillType.Expend)] };
            dto.Incomes = new CategoryGroupItem { Tops = GetTops(records, BillType.Income), List = [.. parents.Where(p => p.Type == BillType.Income)] };
        }
        return Result.Success(dto);

        // 构建常用集合
        List<CategoryWithChildsResult> GetTops(List<FrequencyRecord> records, BillType type)
        {
            var tops = entities.Where(x => x.Top && x.Type == type).Take(10).Select(mapper.Map<CategoryWithChildsResult>).ToList();
            var count = 10 - tops.Count;
            if (count > 0)
            {
                var rs = records.Where(r => !tops.Any(e => e.CategoryId == r.RecordId))
                     .Select(r => new { Record = r, Category = entities.First(e => r.RecordId == e.CategoryId) })
                     .Where(o => o.Category.Type == type)
                     .OrderByDescending((o) => o.Record.Frequency)
                     .Take(count)
                     .Select(o => mapper.Map<CategoryWithChildsResult>(o.Category))
                     .ToList();
                tops.AddRange(rs);
            }

            return tops;
        }
    }
}
