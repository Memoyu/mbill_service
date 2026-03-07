namespace Memo.Bill.Application.Aggregations.Common;

public record AddressResult
{
    /// <summary>
    /// 地点
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在国家 例如：中国
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在省名称 例如：北京市
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在城市名称，请注意：当城市是省直辖县时返回为空，以及城市为北京、上海、天津、重庆四个直辖市时，该字段返回为空；
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在区 例如：海淀区
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在乡镇/街道（此街道为社区街道，不是道路信息） 例如：燕园街道
    /// </summary>
    public string Township { get; set; } = string.Empty;

    /// <summary>
    /// 街道名称 例如：中关村北二条
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// 门牌号 例如：3号
    /// </summary>
    public string StreetNumber { get; set; } = string.Empty;

    /// <summary>
    /// 城市编码 例如：010
    /// </summary>
    public string Citycode { get; set; } = string.Empty;

    /// <summary>
    /// 行政区编码 例如：极110108
    /// </summary>
    public string Adcode { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道编码 例如：110101001000
    /// </summary>
    public string Towncode { get; set; } = string.Empty;
}
