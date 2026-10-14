using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Queries;

[Authorize(Permissions = ApiPermission.Category.ListTop)]
public record ListTopCategoryQuery(BillType Type) : IAuthorizeableRequest<Result>;

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
        var categories = await categoryRepo.Select
            .Where(x => x.Type == request.Type && x.CreateUserId == userId && x.Top)
            .OrderBy(x => x.Sort)
            .Take(10)
            .ToListAsync(cancellationToken) ?? [];

        var tops = categories.Select(mapper.Map<CategoryResult>).ToList();
        // 设置的常用项不足10个时，根据使用记录筛出常用项
        if (tops.Count < 10)
        {
            var ids = tops.Select(x => x.CategoryId).ToList();
            var count = 10 - tops.Count;
            var records = await billPropFreqRepo.Select.From<Category>((r, c) => r.LeftJoin(r => r.RecordId == c.CategoryId))
                .Where((r, c) => r.Type == FrequencyRecordType.Category && c.Type == request.Type && c.CreateUserId == userId)
                .WhereIf(ids.Count > 0, (r, c) => !ids.Contains(r.RecordId))
                .OrderByDescending((r, c) => r.Frequency)
                .Limit(count).ToListAsync((r, c) => c, cancellationToken);
            tops.AddRange(mapper.Map<List<CategoryResult>>(records));
        }
 
        return Result.Success(tops);
    }
}
