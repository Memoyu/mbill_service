namespace Memo.Bill.Application.Common.Models.Settings;

public class JwtOptions
{
    /// <summary>
    /// 密钥
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// 签发人
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// 受众
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Jwt Token 过期时间（分钟） 
    /// </summary>
    public int ExpiryInMin { get; set; } = 120;
}
