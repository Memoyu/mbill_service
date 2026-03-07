namespace Memo.Bill.Application.Accounts.Commands.Update;

[Authorize(Permissions = ApiPermission.Account.Update)]
[Transactional]
public record UpdateAccountCommand(string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("账户名称不能为空");

        RuleFor(x => x.Icon)
          .NotEmpty()
          .WithMessage("账户图标不能为空");
    }
}
