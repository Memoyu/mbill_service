using Memo.Bill.Application.Common.Interfaces.Services.Amap;
using Memo.Bill.Base.Test;
using Memo.Bill.Infrastructure.Services.Amap;
using Microsoft.Extensions.DependencyInjection;

namespace Memo.Bill.Infrastructure.Test.Services;

public class AmapServiceTest : BaseTestConfiguration
{
    private readonly IAmapService _amapService;

    public AmapServiceTest()
    {
        Services.AddHttpClient();
        _amapService = GetTestService<IAmapService, AmapService>();
    }

    [Fact]
    public async Task GetGeocodeRegeoAsync_Should_Success()
    {
        var res = await _amapService.GetGeocodeRegeoAsync("113.345173,23.174131", default);

        Assert.NotNull(res);
        Assert.True(res.IsSuccess);
    }

    [Fact]
    public async Task GetWeatherInfoAsync_Should_Success()
    {
        var res = await _amapService.GetWeatherInfoAsync("440106", default);

        Assert.NotNull(res);
        Assert.True(res.IsSuccess);
    }
}
