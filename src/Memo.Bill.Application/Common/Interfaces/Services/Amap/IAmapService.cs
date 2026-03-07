using Memo.Bill.Application.Common.Models.Services.Amap;

namespace Memo.Bill.Application.Common.Interfaces.Services.Amap;

public interface IAmapService
{
    /// <summary>
    /// 逆地理编码
    /// </summary>
    /// <param name="location">传入内容规则：经度在前，纬度在后，经纬度间以“,”分割，经纬度小数点后不要超过 6 位 示例:116.481488,39.990464</param>
    /// <returns></returns>
    public Task<GetGeocodeRegeoResponse> GetGeocodeRegeoAsync(string location, CancellationToken cancellationToken);

    /// <summary>
    /// 天气查询
    /// </summary>
    /// <param name="city">城市编码，输入城市的adcode</param>
    /// <returns></returns>
    public Task<GetWeatherInfoResponse> GetWeatherInfoAsync(string city, CancellationToken cancellationToken);
}
