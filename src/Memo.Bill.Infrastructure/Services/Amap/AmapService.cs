using Memo.Bill.Application.Common.Extensions;
using Memo.Bill.Application.Common.Interfaces.Services.Amap;
using Memo.Bill.Application.Common.Models.Services.Amap;
using Memo.Bill.Application.Common.Models.Settings;
using Microsoft.Extensions.Options;

namespace Memo.Bill.Infrastructure.Services.Amap;

[AppService(ServiceLifeType = ServiceLifeType.Singleton)]
public class AmapService : IAmapService
{
    private readonly AmapOptions? _options;
    private readonly HttpClient _client;

    public AmapService(IOptionsMonitor<AuthorizationSettings> authOptions, IHttpClientFactory httpClientFactory)
    {
        _options = authOptions.CurrentValue?.Amap;
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri("https://restapi.amap.com/");
    }

    public Task<GetGeocodeRegeoResponse> GetGeocodeRegeoAsync(string location, CancellationToken cancellationToken)
    {
        return GetAsync<GetGeocodeRegeoResponse>($"v3/geocode/regeo?key={GetKey()}&location={location}", cancellationToken);
    }

    public Task<GetWeatherInfoResponse> GetWeatherInfoAsync(string city, CancellationToken cancellationToken)
    {
        return GetAsync<GetWeatherInfoResponse>($"v3/weather/weatherInfo?key={GetKey()}&city={city}", cancellationToken);
    }

    private async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : AmapBaseResponse
    {
        var response = await _client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var resp = content?.ToDesJson<T>() ?? throw new Exception("反序列化高德地图api响应失败");
        if (!resp.IsSuccess) throw new Exception($"高德地图api响应失败：{resp.Info}");
        return resp;
    }

    private string GetKey() => _options?.Key ?? throw new Exception("未配置高德地图api应用key");
}
