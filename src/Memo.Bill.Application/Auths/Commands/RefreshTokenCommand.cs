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
        var cacheKey = CacheKeyConst.UserRefreshToken(request.RefreshToken);
        var cacheValue = await ecProvider.GetAsync<long>(cacheKey, cancellationToken);
        if (!cacheValue.HasValue)
            return Result.Failure("刷新Token已过期，请重新登录");
        var user = await userRepo.Where(u => u.UserId == cacheValue.Value).FirstAsync(cancellationToken);
        if (user == null)
            return Result.Failure("用户不存在，请重新登录");

        var token = await jwtTokenGenerator.GenerateTokenAsync(user, cancellationToken);
        // 删除旧的刷新Token缓存
        await ecProvider.RemoveAsync(cacheKey, cancellationToken);
        return Result.Success(new LoginResult(user.UserId, user.Username, token));
    }
}