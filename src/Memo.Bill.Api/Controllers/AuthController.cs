using Memo.Bill.Application.Auths.Commands;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 用户授权
/// </summary>
/// <param name="mediator"></param>
[AllowAnonymous]
public class AuthController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="request">用户账户、密码</param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<Result> LoginAsync(LoginCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 微信用户登录
    /// </summary>
    /// <param name="request">用户账户、密码</param>
    /// <returns></returns>
    [HttpPost("wx-login")]
    public async Task<Result> WxLoginAsync(LoginWxCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 刷新用户Token
    /// </summary>
    /// <param name="request">刷新token</param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    public async Task<Result> RefreshTokenAsync(RefreshTokenCommand request)
    {
        return await mediator.Send(request);
    }
}
