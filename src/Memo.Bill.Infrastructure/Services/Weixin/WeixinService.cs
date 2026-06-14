using Memo.Bill.Application.Common.Extensions;
using Memo.Bill.Application.Common.Interfaces.Services.Weixin;
using Memo.Bill.Application.Common.Models.Services.Weixin;
using Memo.Bill.Application.Common.Models.Settings;
using Microsoft.Extensions.Options;

namespace Memo.Bill.Infrastructure.Services.Weixin;

[AppService(ServiceLifeType = ServiceLifeType.Singleton)]
public class WeixinService : IWeixinService
{
    private readonly MiniProgramOptions _options;
    private readonly HttpClient _client;

    public WeixinService(IOptionsMonitor<AuthorizationSettings> authOptions, IHttpClientFactory httpClientFactory)
    {
        _options = authOptions.CurrentValue?.MiniProgram ?? throw new Exception("微信小程序配置信息不能为空");
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri("https://api.weixin.qq.com/");
    }

    public async Task<WeixinCode2SessionResponse> Code2SessionAsync(string code)
    {

#if !DEBUG
        var url = $"sns/jscode2session?appid={_options.AppId}&secret={_options.AppSecret}&js_code={code}&grant_type=authorization_code";
        var httpResponse = await _client.GetAsync(url);//请求获取
        httpResponse.EnsureSuccessStatusCode();
        var content = await httpResponse.Content.ReadAsStringAsync();//获取响应内容
#else
        var content = "{\"session_key\":\"cKAHh5rUtZqAryHAS1i7Og == \",\"openid\":\"otPIb4-QEB2eprYBLllCNf425J80\"}";
#endif

        var code2Session = content.ToDesJson<WeixinCode2SessionResponse>();
        if (code2Session?.ErrCode != 0 || string.IsNullOrEmpty(code2Session.OpenId))
            throw new ApplicationException($"请求微信Code2Session返回失败 错误：{content}");

        return code2Session;
    }
}
