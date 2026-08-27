using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Queries;

[Authorize(Permissions = ApiPermission.Category.ListGroup)]
public record ListGroupCategoryQuery() : IAuthorizeableRequest<Result>;

public class ListGroupCategoryQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<ListGroupCategoryQuery, Result>
{
    public async Task<Result> Handle(ListGroupCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await categoryRepo.Select.Where(x => x.CreateUserId == userId).OrderBy(x => x.Sort).ToListAsync(cancellationToken) ?? [];

        var dto = new CategoryGroupsResult();
        if (entities.Count > 0)
        {
            var parents = new List<CategoryGroupItem>();
            var childs = new List<CategoryResult>();
            foreach (var ca in entities)
            {
                if (!ca.ParentId.HasValue)
                    parents.Add(mapper.Map<CategoryGroupItem>(ca));
                else
                    childs.Add(mapper.Map<CategoryResult>(ca));
            }

            parents.ForEach(d =>
            {
                d.Childs = [.. childs.Where(x => x.ParentId == d.CategoryId)];
            });

            dto.Expends = [.. parents.Where(p => p.Type == BillType.Expend)];
            dto.Incomes = [.. parents.Where(p => p.Type == BillType.Income)];
        }
        return Result.Success(dto);
    }
}
