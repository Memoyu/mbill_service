namespace Memo.Bill.Application.Users.Commands.Delete;

[Authorize(Permissions = ApiPermission.User.Delete)]
public record DeleteUserCommand(long UserId ) : IAuthorizeableRequest<Result>;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => x > 0)
            .WithMessage("用户Id必须大于0");

        RuleFor(x => x.UserId)
            .Must(x => x != 1)
            .WithMessage("初始化的管理员无法删除");
    }
}
