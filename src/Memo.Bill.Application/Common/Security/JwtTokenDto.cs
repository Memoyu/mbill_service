namespace Memo.Bill.Application.Common.Security;

public record JwtTokenDto(
    string AccessToken,
    string RefreshToken,
    long ExpiredAt
);