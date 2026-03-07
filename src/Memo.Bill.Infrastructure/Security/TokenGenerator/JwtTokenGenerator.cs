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
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

        var expiryInMin = Convert.ToDouble(_jwtOptions.ExpiryInMin);
        var expiredAt = DateTime.UtcNow.AddMinutes(expiryInMin);
        var securityToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiredAt,
            signingCredentials: signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        string refreshToken = GenerateRefreshToken();
        await ecProvider.SetAsync(CacheKeyConst.UserRefreshToken(refreshToken), user.UserId, TimeSpan.FromMinutes(expiryInMin).Add(TimeSpan.FromDays(5)), cancellationToken);

        var expiredAtSec = (expiredAt.ToUniversalTime().Ticks / TimeSpan.TicksPerSecond) - 62135596800;

        return new JwtTokenDto(accessToken, refreshToken, expiredAtSec);
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
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
