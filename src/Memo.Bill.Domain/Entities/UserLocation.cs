namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 用户地址记录表
/// </summary>
[Table(Name = "user_location")]
[Index("idx_user_location_user_id", nameof(UserId), false)]
public class UserLocation : BaseAuditEntity
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [Description("用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 坐标
    /// </summary>
    [Description("坐标")]
    [Column(StringLength = 100)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 地点
    /// </summary>
    [Description("地点")]
    [Column(StringLength = 200)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在国家 例如：中国
    /// </summary>
    [Description("坐标点所在国家")]
    [Column(StringLength = 20)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在省名称 例如：北京市
    /// </summary>
    [Description("坐标点所在省")]
    [Column(StringLength = 50)]
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在城市名称，请注意：当城市是省直辖县时返回为空，以及城市为北京、上海、天津、重庆四个直辖市时，该字段返回为空；
    /// </summary>
    [Description("坐标点所在城市")]
    [Column(StringLength = 50)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在区 例如：海淀区
    /// </summary>
    [Description("坐标点所在区")]
    [Column(StringLength = 50)]
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// 坐标点所在乡镇/街道（此街道为社区街道，不是道路信息） 例如：燕园街道
    /// </summary>
    [Description("坐标点所在乡镇/街道")]
    [Column(StringLength = 100)]
    public string Township { get; set; } = string.Empty;

    /// <summary>
    /// 街道名称 例如：中关村北二条
    /// </summary>
    [Description("街道名称")]
    [Column(StringLength = 100)]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// 门牌号 例如：3号
    /// </summary>
    [Description("门牌号")]
    [Column(StringLength = 50)]
    public string StreetNumber { get; set; } = string.Empty;

    /// <summary>
    /// 城市编码 例如：010
    /// </summary>
    [Description("城市编码")]
    [Column(StringLength = 10)]
    public string Citycode { get; set; } = string.Empty;

    /// <summary>
    /// 行政区编码 例如：极110108
    /// </summary>
    [Description("行政区编码")]
    [Column(StringLength = 10)]
    public string Adcode { get; set; } = string.Empty;

    /// <summary>
    /// 乡镇街道编码 例如：110101001000
    /// </summary>
    [Description("乡镇街道编码")]
    [Column(StringLength = 20)]
    public string Towncode { get; set; } = string.Empty;
}
