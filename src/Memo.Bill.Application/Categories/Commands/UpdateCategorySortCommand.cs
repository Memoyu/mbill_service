using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Commands;

[Authorize(Permissions = ApiPermission.Category.UpdateSort)]
[Transactional]
public record UpdateCategorySortCommand(List<CategoryUpdateSort> UpdateSorts) : IAuthorizeableRequest<Result>;

public class SortCategoryCommandValidator : AbstractValidator<UpdateCategorySortCommand>
{
    public SortCategoryCommandValidator()
    {
        RuleFor(x => x.UpdateSorts)
             .NotEmpty()
            .Must(x => x.Count > 0)
            .WithMessage("排序对象不能为空");
    }
}

public class SortCategoryCommandHandler(
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<UpdateCategorySortCommand, Result>
{
    public async Task<Result> Handle(UpdateCategorySortCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.UpdateSorts)
        {
            var update = await categoryRepo.Select.Where(x => x.CategoryId == item.CategoryId).FirstAsync(cancellationToken);
            if (update == null) continue;
            update.Id = item.Sort;
            var row = await categoryRepo.UpdateAsync(update, cancellationToken);
            if (row < 1) throw new ApplicationException("更新分类排序失败");
        }

        return Result.Success();
    }
}