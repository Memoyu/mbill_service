using Memo.Bill.Application.Common.Interfaces.Services.Weixin;
using Memo.Bill.Domain.Events.Users;

namespace Memo.Bill.Application.Auths.Commands;

[Transactional]
public record LoginWxCommand(string Code) : IRequest<Result>;

public class LoginWxCommandValidator : AbstractValidator<LoginWxCommand>
{
    public LoginWxCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("微信Code不能为空");
    }
}

public class LoginWxCommandHandler(
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo,
     IBaseDefaultRepository<UserRole> userRoleRepo,
    IJwtTokenGenerator jwtTokenGenerator,
    IWeixinService weixinService
    ) : IRequestHandler<LoginWxCommand, Result>
{
    public async Task<Result> Handle(LoginWxCommand request, CancellationToken cancellationToken)
    {
        var session = await weixinService.Code2SessionAsync(request.Code) ?? throw new ApplicationException("微信登录失败");
        var userIdentity = await userIdentityRepo.Where(u => u.Credential.Equals(session.OpenId) && u.IdentityType == UserIdentityType.WeiXin).FirstAsync(cancellationToken);
        User user;
        // 判断认证信息是否为空
        if (userIdentity == null)
        {
            // 生成用户、认证信息
            var userId = SnowFlakeUtil.NextId();
            user = new User
            {
                UserId = userId,
                Username = userId.ToString(),
                Nickname = "微信用户",
                LastLoginTime = DateTime.Now,
                Status = UserStatus.Normal,
            };

            // 创建用户事件
            user.AddDomainEvent(new CreateUserEvent(userId));

            // 写入用户信息
            await userRepo.InsertAsync(user, cancellationToken);
            await userIdentityRepo.InsertAsync(new UserIdentity
            {
                UserId = userId,
                IdentityType = UserIdentityType.WeiXin,
                Identifier = user.Nickname,
                Credential = session.OpenId,
            }, cancellationToken);
            // 用户角色
            await userRoleRepo.InsertAsync(new UserRole { UserId = userId, RoleId = InitConst.DefaultUserId }, cancellationToken);
        }
        else
        {
            // 登录返回token
            user = await userRepo.Where(u => u.UserId == userIdentity.UserId && u.Status == UserStatus.Normal).FirstAsync(cancellationToken) ??
                throw new ApplicationException("用户不存在或已禁用");
        }

        var token = await jwtTokenGenerator.GenerateTokenAsync(user, cancellationToken);

        return Result.Success(new LoginResult(user.UserId, user.Username, token));
    }
}
