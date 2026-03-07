namespace Memo.Bill.Application.Users.Commands.Update;

[Authorize(Permissions = ApiPermission.User.Update)]
public record UpdateUserCommand(
    long UserId,
    string Username,
    string Nickname,
    string? Avatar,
    string? PhoneNumber,
    string? Email,
    List<long> Roles
    ) : IAuthorizeableRequest<Result>;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
          .Must(x => x > 0)
          .WithMessage("用户Id必须大于0");

        RuleFor(x => x.Username)
            .MinimumLength(1)
            .MaximumLength(20)
            .WithMessage("用户名称长度在1-20个字符之间");


        RuleFor(x => x.Nickname)
            .MinimumLength(1)
            .MaximumLength(20)
            .WithMessage("用户昵称长度在1-20个字符之间");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("邮箱格式有误")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("角色不能为空");
    }
}
