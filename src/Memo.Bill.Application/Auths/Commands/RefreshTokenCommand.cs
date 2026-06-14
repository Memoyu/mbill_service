using EasyCaching.Core;

namespace Memo.Bill.Application.Auths.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result>;


public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("刷新Token不能为空");
    }
}

public class RefreshTokenCommandHandler(
    IBaseDefaultRepository<User> userRepo,
    IEasyCachingProvider ecProvider,
    IJwtTokenGenerator jwtTokenGenerator
    ) : IRequestHandler<RefreshTokenCommand, Result>
{
    public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var cacheValue = await ecProvider.GetAsync<long>(CacheKeyConst.UserRefreshToken(request.RefreshToken), cancellationToken);
        if (!cacheValue.HasValue)
            throw new ApplicationException("刷新Token已过期，请重新登录");
        var user = await userRepo.Where(u => u.UserId == cacheValue.Value).FirstAsync(cancellationToken) ??
                throw new ApplicationException("用户不存在");

        var token = await jwtTokenGenerator.GenerateTokenAsync(user, cancellationToken);
        return Result.Success(new LoginResult(user.UserId, user.Username, token.AccessToken, token.RefreshToken, token.ExpiredAt));
    }
}