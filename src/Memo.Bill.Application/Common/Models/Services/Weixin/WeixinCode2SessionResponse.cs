using System.Text.Json.Serialization;

namespace Memo.Bill.Application.Common.Models.Services.Weixin;

public record WeixinCode2SessionResponse
{
    /// <summary>
    /// 微信OpenId
    /// </summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;

    /// <summary>
    /// SessionKey
    /// </summary>
    [JsonPropertyName("session_key")]
    public string SessionKey { get; set; } = string.Empty;

    /// <summary>
    /// 微信UnionId
    /// </summary>
    [JsonPropertyName("unionid")]
    public string UnionId { get; set; } = string.Empty;

    /// <summary>
    /// 错误编码
    /// </summary>
    [JsonPropertyName("errcode")]
    public int ErrCode { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [JsonPropertyName("errmsg")]
    public string ErrMsg { get; set; } = string.Empty;
}
