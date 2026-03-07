namespace Memo.Bill.Application.Accounts.Commands.Delete;

[Authorize(Permissions = ApiPermission.Account.Delete)]
[Transactional]
public record DeleteAccountCommand(long AccountId) : IAuthorizeableRequest<Result>;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");
    }
}
