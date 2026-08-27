using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Queries;

[Authorize(Permissions = ApiPermission.Category.ListTop)]
public record ListTopCategoryQuery() : IAuthorizeableRequest<Result>;

public class TopCategoryQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<FrequencyRecord> billPropFreqRepo
    ) : IRequestHandler<ListTopCategoryQuery, Result>
{
    public async Task<Result> Handle(ListTopCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await categoryRepo.Select.Where(x => x.CreateUserId == userId && x.Top).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var dto = new CategoryTopsResult();
        var ids = new List<long>();
        var expendTops = new List<CategoryResult>();
        var incomeTops = new List<CategoryResult>();
        if (entities.Count > 0)
        {
            foreach (var ca in entities)
            {
                if (!ca.Top) continue;

                if (ca.Type == BillType.Expend && expendTops.Count < 10)
                    expendTops.Add(mapper.Map<CategoryResult>(ca));
                if (ca.Type == BillType.Income && expendTops.Count < 10)
                    incomeTops.Add(mapper.Map<CategoryResult>(ca));
                ids.Add(ca.CategoryId);
            }
        }

        // 设置的常用项不足10个时，根据使用记录筛出常用项
        // 支出
        if (expendTops.Count < 10)
        {
            var count = 10 - expendTops.Count;
            var records = await billPropFreqRepo.Select.From<Category>((r, c) => r.LeftJoin(r => r.RecordId == c.CategoryId))
                .Where((r, c) => r.Type == FrequencyRecordType.Category && c.Type == BillType.Expend && c.CreateUserId == userId)
                .WhereIf(ids.Count > 0, (r, c) => !ids.Contains(r.RecordId))
                .OrderByDescending((r, c) => r.Frequency)
                .Limit(count).ToListAsync((r, c) => c, cancellationToken);
            expendTops.AddRange(mapper.Map<List<CategoryResult>>(records));
        }

        //收入
        if (incomeTops.Count < 10)
        {
            var count = 10 - incomeTops.Count;
            var records = await billPropFreqRepo.Select.From<Category>((r, c) => r.LeftJoin(r => r.RecordId == c.CategoryId))
               .Where((r, c) => r.Type == FrequencyRecordType.Category && c.Type == BillType.Income && c.CreateUserId == userId)
               .WhereIf(ids.Count > 0, (r, c) => !ids.Contains(r.RecordId))
               .OrderByDescending((r, c) => r.Frequency)
               .Limit(count).ToListAsync((r, c) => c, cancellationToken);
            incomeTops.AddRange(mapper.Map<List<CategoryResult>>(records));
        }

        dto.Expends = expendTops;
        dto.Incomes = incomeTops;

        return Result.Success(dto);
    }
}
