using Autofac;
using mbill_service.Core.Common.Configs;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Consts;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Exceptions;
using mbill_service.Service.Core.Auth;
using mbill_service.Service.Core.Auth.Input;
using mbill_service.Service.Core.User;
using mbill_service.Service.Core.User.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using mbill_service.Service.Core.Wx;

namespace mbill_service.Controllers.Core
{
    /// <summary>
    /// 账户相关
    /// </summary>
    [Route("api/account")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IWxService _wxSvc;

        public AccountController(IComponentContext componentContext, IAccountService accountService, IUserService userService, IWxService wxSvc)
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
        /// 微信登录接口
        /// </summary>
        [HttpPost("wxlogin")]
        public async Task<ServiceResult<TokenDto>> WxLogin(WxLoginDto loginDto)
        {
            return await _accountService.WxLoginAsync(loginDto);
        }

        /// <summary>
        /// 获取用户信息，By Id
        /// </summary>
        [HttpGet("user")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
        public async Task<ServiceResult<UserDto>> GetByIdAsync([FromQuery] long? id)
        {
            return ServiceResult<UserDto>.Successed(await _userService.GetAsync(id));
        }

        /// <summary>
        /// 刷新用户的token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh")]
        public async Task<ServiceResult<TokenDto>> GetRefreshToken()
        {
            string? refreshToken = Request.Headers["refresh_token"];
            if (refreshToken == null)
            {
                throw new KnownException("请先登录.", ServiceResultCode.RefreshTokenError);
            }
            return await _accountService.GetTokenByRefreshAsync(refreshToken);
        }

    }
}
