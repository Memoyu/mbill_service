namespace mbill.Controllers.Core;

/// <summary>
/// 账户相关
/// </summary>
[Route("api/account")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
public class AccountController : ApiControllerBase
{
    private readonly IAccountSvc _accountService;
    private readonly IUserSvc _userService;
    private readonly IWxSvc _wxSvc;

    public AccountController(IComponentContext componentContext, IAccountSvc accountService, IUserSvc userService, IWxSvc wxSvc)
    {
        _accountService = accountService;
        _userService = userService;
        _wxSvc = wxSvc;
    }

    /// <summary>
    /// 登录接口
    /// </summary>
    ///<example>
    /// 用户名：admin，密码：123456
    /// </example>
    [HttpPost("login")]
    public async Task<ServiceResult<TokenDto>> Login(AccountLoginDto loginDto)
    {
        return await _accountService.AccountLoginAsync(loginDto);
    }

    /// <summary>
    /// 微信预登录登录接口
    /// </summary>
    [HttpGet("wx-pre-login")]
    public async Task<ServiceResult<PreLoginUserDto>> WxPreLogin([FromQuery]string code)
    {
        return await _accountService.WxPreLoginAsync(code);
    }

    /// <summary>
    /// 微信登录接口
    /// </summary>
    [HttpPost("wxlogin")]
    public async Task<ServiceResult<TokenWithUserDto>> WxLogin(WxLoginInput loginDto)
    {
        return await _accountService.WxLoginAsync(loginDto);
    }

    /// <summary>
    /// 刷新用户的token
    /// </summary>
    /// <returns></returns>
    [HttpGet("refresh")]
    public async Task<ServiceResult<TokenDto>> GetRefreshToken()
    {
        string refreshToken = Request.Headers["refresh-token"];
        if (refreshToken == null)
        {
            throw new KnownException("请先登录.", ServiceResultCode.RefreshTokenError);
        }
        return await _accountService.GetTokenByRefreshAsync(refreshToken);
    }

    /// <summary>
    /// 获取用户信息，By Id
    /// </summary>
    [HttpGet("user")]
    [Authorize]
    [LocalAuthorize("获取用户信息", "用户")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    public async Task<ServiceResult<UserDto>> GetByIdAsync([FromQuery] long? id)
    {
        return await _userService.GetAsync(id);
    }

    /// <summary>
    /// 更新账户信息
    /// </summary>
    /// <param name="input">账单信息</param>
    [HttpPut]
    [Authorize]
    [LocalAuthorize("更新用户信息", "用户")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v1)]
    public async Task<ServiceResult> UpdateAsync([FromBody] ModifyUserBaseDto input)
    {
        return await _userService.UpdateAsync(input);
    }

}
