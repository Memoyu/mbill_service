namespace Memo.Bill.Application.Tokens.Common;

public record LoginResult(
    long UserId,
    string Username,
    string AccessToken,
    string RefreshToken,
    long AccessExpiresIn
);
