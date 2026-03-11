using System.Text.Json.Serialization;

namespace Memo.Bill.Application.Common.Models.Services.Amap;

public class GetWeatherInfoResponse : AmapBaseResponse
{
    /// <summary>
    /// 天气信息结果
    /// </summary>
    public List<WeatherLive> Lives { get; set; } = new();

}

public class WeatherLive
{
    /// <summary>
    /// 省份名
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 城市名
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// 区域编码
    /// </summary>
    public string Adcode { get; set; } = string.Empty;

    /// <summary>
    /// 天气现象（汉字描述）
    /// </summary>
    public string Weather { get; set; } = string.Empty;

    /// <summary>
    /// 实时气温，单位：摄氏度
    /// </summary>
    public string Temperature { get; set; } = string.Empty;

    /// <summary>
    /// 风向描述
    /// </summary>
    [JsonPropertyName("winddirection")]
    public string WindDirection { get; set; } = string.Empty;

    /// <summary>
    /// 风力级别，单位：级
    /// </summary>
    [JsonPropertyName("windpower")]
    public string WindPower { get; set; } = string.Empty;

    /// <summary>
    /// 空气湿度
    /// </summary>
    public string Humidity { get; set; } = string.Empty;

    /// <summary>
    /// 数据发布的时间
    /// </summary>
    [JsonPropertyName("reporttime")]
    public string ReportTime { get; set; } = string.Empty;

    /// <summary>
    /// 实时气温，浮点数
    /// </summary>
    [JsonPropertyName("temperature_float")]
    public string TemperatureFloat { get; set; } = string.Empty;

    /// <summary>
    /// 空气湿度，浮点数
    /// </summary>
    [JsonPropertyName("humidity_float")]
    public string HumidityFloat { get; set; } = string.Empty;
}
