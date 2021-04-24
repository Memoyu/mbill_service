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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace mbill_service.Controllers.Core
{
    /// <summary>
    /// 账户相关
    /// </summary>
    [Route("api/account")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    public class AccountController : ApiControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public AccountController(IComponentContext componentContext, IAccountService accountService, IUserService userService)
        {
            bool isIdentityServer4 = Appsettings.IdentityServer4Enable;
            _tokenService = componentContext.ResolveNamed<ITokenService>(isIdentityServer4 ? typeof(IdentityServer4Service).Name : typeof(JwtTokenService).Name);
            _accountService = accountService;
            _userService = userService;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        ///<example>
        /// 用户名：admin，密码：123456
        /// </example>
        [HttpPost("login")]
        public async Task<ServiceResult<TokenDto>> Login(LoginDto loginDto)
        {
            return ServiceResult<TokenDto>.Successed(await _tokenService.LoginAsync(loginDto));
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
        public async Task<TokenDto> GetRefreshToken()
        {
            string refreshToken;
            string authorization = Request.Headers["Authorization"];

            if (authorization != null && authorization.StartsWith(JwtBearerDefaults.AuthenticationScheme))//判断请求是否带Token
            {
                refreshToken = authorization.Substring(JwtBearerDefaults.AuthenticationScheme.Length + 1).Trim();//获取refreshToken
            }
            else
            {
                throw new KnownException(" 请先登录.", ServiceResultCode.RefreshTokenError);
            }
            return await _tokenService.GetTokenByRefreshAsync(refreshToken);
        }

    }
}
