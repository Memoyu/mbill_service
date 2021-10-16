using Newtonsoft.Json;

namespace mbill_service.Service.Core.Wx.Output
{
    public class WxAccessTokenDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
