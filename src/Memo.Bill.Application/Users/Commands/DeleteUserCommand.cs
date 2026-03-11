namespace Memo.Bill.Application.Users.Commands;

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

public class DeleteUserCommandHandler(
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo,
    IBaseDefaultRepository<UserRole> userRoleRepo
    ) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.Select.Where(c => c.UserId == request.UserId).FirstAsync(cancellationToken) ?? throw new ApplicationException("用户不存在");

        var uiAffrows = await userIdentityRepo.DeleteAsync(ui => ui.UserId == request.UserId, cancellationToken);
        if (uiAffrows <= 0) throw new ApplicationException("删除用户认证失败");

        await userRoleRepo.DeleteAsync(ui => ui.UserId == request.UserId, cancellationToken);

        var affrows = await userRepo.DeleteAsync(user, cancellationToken);

        return affrows > 0 ? Result.Success() : throw new ApplicationException("删除用户失败");
    }
}