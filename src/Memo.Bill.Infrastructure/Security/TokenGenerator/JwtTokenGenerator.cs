using EasyCaching.Core;
using Memo.Bill.Application.Common.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Memo.Bill.Infrastructure.Security.TokenGenerator;

public class JwtTokenGenerator(
    IEasyCachingProvider ecProvider,
    IOptionsMonitor<AuthorizationSettings> authOptions) : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = authOptions.CurrentValue?.Jwt ?? throw new Exception("未配置服务jwt授权信息");

    public async Task<JwtTokenDto> GenerateTokenAsync(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

        var now = DateTime.UtcNow;
        var expiryInMin = Convert.ToInt32(_jwtOptions.ExpiryInMin);
        // 转换成秒
        var accessExpiresIn = expiryInMin * 60;
        var securityToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: now.AddSeconds(accessExpiresIn),
            signingCredentials: signingCredentials);

        // 生成access token
        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

        // 生成 refresh token
        // 在 access token 过期后的两天后过期
        var refreshExpiresIn = accessExpiresIn + (2 * 24 * 60 * 60);
        string refreshToken = GenerateRefreshToken();
        await ecProvider.SetAsync(CacheKeyConst.UserRefreshToken(refreshToken), user.UserId, TimeSpan.FromSeconds(refreshExpiresIn), cancellationToken);

        return new JwtTokenDto(accessToken, refreshToken, accessExpiresIn, refreshExpiresIn);
    }

    public async Task<JwtTokenDto> RefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var jwtToken = await GenerateTokenAsync(user, cancellationToken);
        return jwtToken;
    }

    /// <summary>
    /// 生成RefreshToken
    /// </summary>
    /// <param name="size">长度</param>
    /// <returns></returns>
    private string GenerateRefreshToken(int size = 32)
    {
        var randomNumber = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
