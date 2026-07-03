namespace Memo.Bill.Application.Categories.Commands;

public record SortCategoryDto(long CategoryId, int Sort);

[Authorize(Permissions = ApiPermission.Category.Sort)]
[Transactional]
public record SortCategoryCommand(List<SortCategoryDto> Sorts) : IAuthorizeableRequest<Result>;

public class SortCategoryCommandValidator : AbstractValidator<SortCategoryCommand>
{
    public SortCategoryCommandValidator()
    {
        RuleFor(x => x.Sorts)
             .NotEmpty()
            .WithMessage("排序分类不能为空");
    }
}

public class SortCategoryCommandHandler(
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<SortCategoryCommand, Result>
{
    public async Task<Result> Handle(SortCategoryCommand request, CancellationToken cancellationToken)
    {
        // 更新排序
        var categoryIds = request.Sorts.Select(s => s.CategoryId).ToList();
        var categories = await categoryRepo.Select.Where(c => categoryIds.Contains(c.CategoryId)).ToListAsync(cancellationToken) ?? [];
        categories.ForEach(c =>
        {
            c.Sort = request.Sorts.FirstOrDefault(s => s.CategoryId == c.CategoryId)?.Sort ?? c.Sort;
        });
        var rows = await categoryRepo.UpdateAsync(categories, cancellationToken);

        return rows > 0 ? Result.Success() : throw new ApplicationException("更新分类排序失败");
    }
}