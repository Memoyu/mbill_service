using Memo.Bill.Application.Categories.Queries.Get;

namespace Memo.Bill.Application.Accounts.Queries.Get;

public class GetCategoryQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Category> categorytRepo
    ) : IRequestHandler<GetCategoryQuery, Result>
{
    public async Task<Result> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
