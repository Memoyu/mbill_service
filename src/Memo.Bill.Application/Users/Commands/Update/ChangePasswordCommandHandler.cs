namespace Memo.Bill.Application.Users.Commands.Update;

public class ChangePasswordCommandHandler(
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var entity = await userRepo.Select.Where(u => u.UserId == request.UserId).FirstAsync(cancellationToken) ?? throw new ApplicationException("用户不存在");

        var userIdentity = await userIdentityRepo.Select.Where(u => u.UserId == request.UserId).FirstAsync(cancellationToken);
        if (userIdentity.IdentityType != Domain.Enums.UserIdentityType.Password) throw new ApplicationException("该用户并非使用密码认证，无法变更密码");

        userIdentity.Credential = EncryptUtil.Encrypt(request.Password);
        var affrow = await userIdentityRepo.UpdateAsync(userIdentity, cancellationToken);
        return affrow <= 0 ? throw new ApplicationException("变更用户密码失败") : Result.Success();
    }
}
