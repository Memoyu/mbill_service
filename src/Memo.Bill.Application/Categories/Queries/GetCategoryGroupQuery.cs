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

        var entities = await categoryRepo.Select.Where(c => c.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<CategoryGroupResult>>(entities.Where(c => !c.ParentId.HasValue).OrderByDescending(d => d.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<CategoryResult>>(entities.Where(d => d.ParentId == d.CategoryId).OrderByDescending(d => d.Sort));
        });
        return Result.Success(dtos);
    }
}
