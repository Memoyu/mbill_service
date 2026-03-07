namespace Memo.Bill.Application.Categories.Commands.Delete;

[Authorize(Permissions = ApiPermission.Category.Delete)]
[Transactional]
public record DeleteCategoryCommand(long AccountId) : IAuthorizeableRequest<Result>;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");
    }
}
