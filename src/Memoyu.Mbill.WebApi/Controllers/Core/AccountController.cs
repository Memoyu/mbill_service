/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Controllers.Core
*   文件名称 ：AccountController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 11:57:14
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Application.Core;
using Memoyu.Mbill.Application.Core.Impl;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Controllers.Core
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
        public AccountController(IComponentContext componentContext, IAccountService accountService)
        {
            bool isIdentityServer4 = AppSettings.IdentityServer4Enable;
            _tokenService = componentContext.ResolveNamed<ITokenService>(isIdentityServer4 ? typeof(IdentityServer4Service).Name : typeof(JwtTokenService).Name);
            _accountService = accountService;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        ///<example>
        /// 用户名：admin，密码：123456
        /// </example>
        [HttpPost("login")]
        public async Task<ServiceResult<TokenDto>> Login(LoginInputDto loginInputDto)
        {
            return ServiceResult<TokenDto>.Successed(await _tokenService.LoginAsync(loginInputDto));
        }

        /// <summary>
        /// 刷新用户的token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh")]
        public async Task<ServiceResult<TokenDto>> GetRefreshToken()
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
            return ServiceResult<TokenDto>.Successed(await _tokenService.GetTokenByRefreshAsync(refreshToken));
        }

    }
}
