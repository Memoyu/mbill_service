namespace Memo.Bill.Application.Aggregations.Common;

public class WeatherInfoResult
{
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
    public string WindDirection { get; set; } = string.Empty;

    /// <summary>
    /// 风力级别，单位：级
    /// </summary>
    public string WindPower { get; set; } = string.Empty;

    /// <summary>
    /// 空气湿度
    /// </summary>
    public string Humidity { get; set; } = string.Empty;

    /// <summary>
    /// 数据发布的时间
    /// </summary>
    public string ReportTime { get; set; } = string.Empty;
}
