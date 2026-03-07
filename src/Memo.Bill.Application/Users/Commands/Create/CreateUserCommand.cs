namespace Memo.Bill.Application.Users.Commands.Create;

[Authorize(Permissions = ApiPermission.User.Create)]
[Transactional]
public record CreateUserCommand(
    string Username,
    string Nickname,
    UserIdentityType UserIdentityType,
    string Credential,
    string? Avatar,
    string? PhoneNumber,
    string? Email,
    List<long> Roles
    ) : IAuthorizeableRequest<Result>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(1)
            .MaximumLength(20)
            .WithMessage("用户名称长度在1-20个字符之间");

        RuleFor(x => x.Nickname)
            .MinimumLength(1)
            .MaximumLength(20)
            .WithMessage("用户昵称长度在1-20个字符之间");

        RuleFor(x => x.UserIdentityType)
            .IsInEnum()
            .WithMessage("用户认证方式错误");

        RuleFor(x => x.Credential)
          .Matches("^[A-Za-z0-9_*&$#@]{4,20}$")
          .WithMessage("密码长度必须在6~22位之间，包含字符、数字和 _")
          .When(x => !string.IsNullOrWhiteSpace(x.Credential) && x.UserIdentityType == UserIdentityType.Password);

        RuleFor(x => x)
          .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.PhoneNumber))
          .WithMessage("认证方式不为密码时，需要填入邮箱或电话")
          .When(x => x.UserIdentityType != UserIdentityType.Password);

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("邮箱格式有误")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("角色不能为空");
    }
}
