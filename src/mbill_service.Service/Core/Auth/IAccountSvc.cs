namespace mbill_service.Service.Core.Auth;

public interface IAccountSvc : IApplicationSvc
{
    /// <summary>
    /// 账户登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<TokenDto>> AccountLoginAsync(AccountLoginDto input);

    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<TokenWithUserDto>> WxLoginAsync(WxLoginInput input);

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<ServiceResult<TokenDto>> GetTokenByRefreshAsync(string refreshToken);
}
