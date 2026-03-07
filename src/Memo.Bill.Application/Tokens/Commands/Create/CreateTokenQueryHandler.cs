namespace Memo.Bill.Application.Tokens.Commands.Create;

public class CreateTokenHandler(
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo,
    IJwtTokenGenerator jwtTokenGenerator
    ) : IRequestHandler<CreateTokenQuery, Result>
{
    public async Task<Result> Handle(CreateTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepo.Where(u => u.Username.Equals(request.Username)).FirstAsync(cancellationToken) ??
            throw new ApplicationException("用户名或密码错误");
        
        var identity = await userIdentityRepo.Where(u => u.UserId == user.UserId).FirstAsync(cancellationToken);
        if (identity is null || !identity.Credential.Equals(EncryptUtil.Encrypt(request.Password)))
            throw new ApplicationException("用户名或密码错误");

        var token = await jwtTokenGenerator.GenerateTokenAsync(user, cancellationToken);

        // 更新最后一次登录时间
        user.LastLoginTime = DateTime.Now;
        await userRepo.UpdateAsync(user, cancellationToken);

        return Result.Success(new CreateTokenResult(user.UserId, user.Username, token.AccessToken, token.RefreshToken));
    }
}
