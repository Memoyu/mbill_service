using System;

namespace mbill_service.Service.Core.Auth.Input
{
    public class TokenDto
    {
        public TokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public override string ToString()
        {
            return $"TokenDto - 授权Token:{AccessToken},刷新Token:{RefreshToken}";
        }
    }
}
