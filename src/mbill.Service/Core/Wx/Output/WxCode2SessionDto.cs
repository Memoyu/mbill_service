namespace Mbill.Service.Core.Wx.Output;

public class WxCode2SessionDto
{
    public string OpenId { get; set; }

    [JsonProperty("session_key")]
    public string SessionKey { get; set; }

    public string UnionId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}