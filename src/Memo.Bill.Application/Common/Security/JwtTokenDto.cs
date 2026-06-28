namespace Memo.Bill.Application.Common.Security;

public record JwtTokenDto(
    string AccessToken,
    string RefreshToken,
    long AccessExpiresIn, // 访问令牌有效期(秒)
    long RefreshExpiresIn // 刷新令牌有效期(秒)
);