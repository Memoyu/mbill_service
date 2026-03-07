namespace Memo.Bill.Application.Categories.Commands.Update;

[Authorize(Permissions = ApiPermission.Category.Update)]
[Transactional]
public record UpdateCategoryCommand(string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("分类名称不能为空");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("分类图标不能为空");
    }
}
