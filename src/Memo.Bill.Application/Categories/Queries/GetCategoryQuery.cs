using Memo.Bill.Application.Categories.Common;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Categories.Queries;

/// <summary>
/// 获取分类
/// </summary>
/// <param name="CategoryId">分类Id</param>
/// <param name="Parent">是否获取父级</param>
[Authorize(Permissions = ApiPermission.Category.Get)]
public record GetCategoryQuery(long CategoryId, bool? Parent) : IAuthorizeableRequest<Result>;

public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");
    }
}

public class GetCategoryQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categorytRepo
    ) : IRequestHandler<GetCategoryQuery, Result>
{
    public async Task<Result> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entity = await categorytRepo.Select.Where(x => x.CategoryId == request.CategoryId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("分类不存在或已删除");

        var dto = mapper.Map<CategoryResult>(entity);
        if (request.Parent == true)
        {
            var parent = await categorytRepo.Select.Where(x => x.CategoryId == entity.ParentId).FirstAsync(cancellationToken)
                ?? throw new ApplicationException("父分类不存在或已删除");
            dto.Parent = mapper.Map<CategoryResult>(parent);
        }

        return Result.Success(dto);
    }
}
