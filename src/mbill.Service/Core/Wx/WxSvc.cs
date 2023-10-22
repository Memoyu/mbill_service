namespace Mbill.Service.Core.Wx;

public class WxSvc : IWxSvc
{
    private static string WxJscode(string appid, string secret, string code) => $"https://api.weixin.qq.com/sns/jscode2session?appid={appid}&secret={secret}&js_code={code}&grant_type=authorization_code";

    private readonly ILogger<WxSvc> _logger;
    private readonly IHttpClientFactory _httpClient;

    public WxSvc(ILogger<WxSvc> logger, IHttpClientFactory httpClient, IUserRepo userRepo, IUserIdentityRepo userIdentityRepo, IJwtService jsonWebTokenService)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<ServiceResult<WxCode2SessionDto>> GetCode2Session(string code)
    {
#if !DEBUG
            var url = WxJscode(Appsettings.MinPro.AppID, Appsettings.MinPro.AppSecret, code);
            using var client = _httpClient.CreateClient();//创建HttpClient请求
            var httpResponse = await client.GetAsync(url);//请求获取
            if (httpResponse.StatusCode != HttpStatusCode.OK)//判断请求响应是否成功
                return ServiceResult<WxCode2SessionDto>.Failed($"请求微信Code2Session响应失败 错误：{httpResponse.Content.ReadAsStringAsync()}");
            var content = await httpResponse.Content.ReadAsStringAsync();//获取响应内容
#else
        var content = "{\"session_key\":\"cKAHh5rUtZqAryHAS1i7Og == \",\"openid\":\"otPIb4-QEB2eprYBLllCNf425J80\"}";
#endif
        await Task.CompletedTask;
        var code2Session = content.FromJson<WxCode2SessionDto>();
        if (code2Session.ErrCode != 0)
            return ServiceResult<WxCode2SessionDto>.Failed($"请求微信Code2Session返回失败 错误：{content}");
        return ServiceResult<WxCode2SessionDto>.Successed(code2Session);
    }
}
