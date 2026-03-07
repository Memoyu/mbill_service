namespace Memo.Bill.Application.Users.Commands.Update;

[Authorize(Permissions = ApiPermission.User.ChangePassword)]
public record ChangePasswordCommand(long UserId, string Password) : IAuthorizeableRequest<Result>;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
         .Must(x => x > 0)
         .WithMessage("用户Id必须大于0");

        RuleFor(x => x.Password)
          .Matches("^[A-Za-z0-9_*&$#@]{4,20}$")
          .WithMessage("密码长度必须在6~22位之间，包含字符、数字和 _");

    }
}

