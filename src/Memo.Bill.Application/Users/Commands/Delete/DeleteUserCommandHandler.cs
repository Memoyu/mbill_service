namespace Memo.Bill.Application.Users.Commands.Delete;

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
