/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Core.Account.Impl
*   文件名称 ：IdentityServer4Service.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:14:03
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Domain.IRepositories.User;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Core.Impl
{
    public class IdentityServer4Service : ITokenService
    {
        private readonly ILogger<IdentityServer4Service> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public IdentityServer4Service(ILogger<IdentityServer4Service> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
      
        public Task<TokenDto> LoginAsync(LoginInputDto loginInputDto)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> GetTokenByRefreshAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
