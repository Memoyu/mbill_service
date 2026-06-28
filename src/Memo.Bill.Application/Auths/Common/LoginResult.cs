namespace Memo.Bill.Application.Tokens.Common;

public record LoginResult
{
    public long UserId { get; set; }

    public string Username { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    /// <summary>
    /// 访问令牌有效期(秒)
    /// </summary>
    public long AccessExpiresIn { get; set; }
    /// <summary>
    /// 刷新令牌有效期(秒)
    /// </summary>
    public long RefreshExpiresIn { get; set; }

    public LoginResult(long userId, string username, JwtTokenDto token)
    {
        UserId = userId;
        Username = username;
        AccessToken = token.AccessToken;
        AccessExpiresIn = token.AccessExpiresIn;
        RefreshToken = token.RefreshToken;
        RefreshExpiresIn = token.RefreshExpiresIn;
    }
};
