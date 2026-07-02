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

        var entities = await categoryRepo.Select.Where(x => x.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<CategoryGroupResult>>(entities.Where(x => !x.ParentId.HasValue).OrderBy(x => x.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<CategoryResult>>(entities.Where(x => x.ParentId == d.CategoryId).OrderBy(x => x.Sort));
        });
        return Result.Success(dtos);
    }
}
