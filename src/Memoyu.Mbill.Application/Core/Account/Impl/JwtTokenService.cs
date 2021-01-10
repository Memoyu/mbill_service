/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Core.Account.Impl
*   文件名称 ：JwtTokenService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:13:48
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using DotNetCore.Security;
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Application.Contracts.Exceptions;
using Memoyu.Mbill.Application.User;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.IRepositories.User;
using Memoyu.Mbill.Domain.Shared.Security;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core.Account.Impl
{
    public class JwtTokenService : ITokenService
    {
        private readonly ILogger<JwtTokenService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        public JwtTokenService(IUserRepository userRepository, ILogger<JwtTokenService> logger, IUserIdentityService userIdentityService, IJsonWebTokenService jsonWebTokenService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userIdentityService = userIdentityService;
            _jsonWebTokenService = jsonWebTokenService;
        }
        public async Task<TokenDto> LoginAsync(LoginInputDto loginInputDto)
        {
            _logger.LogInformation("JwtLogin");

            UserEntity user = await _userRepository.GetUserAsync(r => r.Username == loginInputDto.Username || r.Email == loginInputDto.Username);

            if (user == null)
            {
                throw new KnownException("用户不存在", ServiceResultCode.NotFound);
            }

            bool valid = await _userIdentityService.VerifyUserPasswordAsync(user.Id, loginInputDto.Password);

            if (!valid)
            {
                throw new KnownException("请输入正确密码", ServiceResultCode.ParameterError);
            }

            _logger.LogInformation($"用户{loginInputDto.Username},登录成功");

            TokenDto tokens = await CreateTokenAsync(user);
            return tokens;
        }

        public Task<TokenDto> GetTokenByRefreshAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建Token和RefreshToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<TokenDto> CreateTokenAsync(UserEntity user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim (ClaimTypes.Email, user.Email?? ""),
                new Claim (ClaimTypes.GivenName, user.Nickname?? ""),
                new Claim (ClaimTypes.Name, user.Username?? ""),
            };
            user.Roles?.ForEach(r =>
            {
                claims.Add(new Claim(ClaimTypes.Role, r.Name));
                claims.Add(new Claim(CoreClaimTypes.Groups, r.Id.ToString()));
            });

            string token = _jsonWebTokenService.Encode(claims);

            string refreshToken = GenerateToken();
            user.ChangeLoginStatus(refreshToken);
            await _userRepository.UpdateAsync(user);

            return new TokenDto(token, refreshToken);
        }

        /// <summary>
        /// 生成RefreshToken
        /// </summary>
        /// <param name="size">长度</param>
        /// <returns></returns>
        private string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
