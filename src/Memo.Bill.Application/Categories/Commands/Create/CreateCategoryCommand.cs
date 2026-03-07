namespace Memo.Bill.Application.Categories.Commands.Create;

[Authorize(Permissions = ApiPermission.Category.Create)]
[Transactional]
public record CreateCategoryCommand(string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("分类名称不能为空");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("分类图标不能为空");
    }
}
