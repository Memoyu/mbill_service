using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Categories.Commands;

[Authorize(Permissions = ApiPermission.Category.Delete)]
[Transactional]
public record DeleteCategoryCommand(long CategoryId) : IAuthorizeableRequest<Result>;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");
    }
}

public class DeleteCategoryCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entity = await categoryRepo.Select.Where(x => x.CategoryId == request.CategoryId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("分类不存在或已删除");

        var row = await categoryRepo.DeleteAsync(entity.CategoryId, cancellationToken);
        if (row < 1) return Result.Failure("删除分类失败");
        // 删除分类为父级分类，需要删除子级分类
        if (!entity.ParentId.HasValue)
            await categoryRepo.DeleteAsync(x => x.ParentId == entity.ParentId, cancellationToken);

        return Result.Success();
    }
}