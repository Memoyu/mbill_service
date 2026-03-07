namespace Memo.Bill.Application.Accounts.Commands.Create;

[Authorize(Permissions = ApiPermission.Account.Create)]
[Transactional]
public record CreateAccountCommand(string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("账户名称不能为空");

        RuleFor(x => x.Icon)
          .NotEmpty()
          .WithMessage("账户图标不能为空");
    }
}
