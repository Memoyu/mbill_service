namespace Memo.Bill.Application.Auths.Commands;

public record LoginCommand(string Username, string Password) : IRequest<Result>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("用户名不能为空");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("密码不能为空");
    }
}

public class LoginCommandHandler(
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo,
    IJwtTokenGenerator jwtTokenGenerator
    ) : IRequestHandler<LoginCommand, Result>
{
    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.Where(u => u.Username.Equals(request.Username) && u.Status == UserStatus.Normal).FirstAsync(cancellationToken) ??
            throw new ApplicationException("用户名或密码错误");

        var identity = await userIdentityRepo.Where(u => u.UserId == user.UserId).FirstAsync(cancellationToken);
        if (identity is null || !identity.Credential.Equals(EncryptUtil.Encrypt(request.Password)))
            throw new ApplicationException("用户名或密码错误");

        var token = await jwtTokenGenerator.GenerateTokenAsync(user, cancellationToken);

        // 更新最后一次登录时间
        user.LastLoginTime = DateTime.Now;
        await userRepo.UpdateAsync(user, cancellationToken);

        return Result.Success(new LoginResult(user.UserId, user.Username, token));
    }
}
