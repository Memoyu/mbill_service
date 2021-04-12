using mbill_service.Service.Core.Auth.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace mbill_service.Service.Core.Auth
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
      
        public Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> GetTokenByRefreshAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
