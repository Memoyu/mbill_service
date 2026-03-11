using Memo.Bill.Application.Categories.Common;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Categories.Queries;

[Authorize(Permissions = ApiPermission.Category.GetGroup)]
public record GetCategoryGroupQuery() : IAuthorizeableRequest<Result>;

public class GetCategoryGroupQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<GetCategoryGroupQuery, Result>
{
    public async Task<Result> Handle(GetCategoryGroupQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entities = await categoryRepo.Select.Where(x => x.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<CategoryGroupResult>>(entities.Where(x => x.ParentId.HasValue).OrderByDescending(x => x.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<CategoryResult>>(entities.Where(x => x.ParentId == d.CategoryId).OrderByDescending(x => x.Sort));
        });
        return Result.Success(dtos);
    }
}
