namespace mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 预购清单实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_pre_order")]
public class PreOrderEntity : FullAduitEntity
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 资产Id
    /// </summary>
    public long AssetId { get; set; }

    /// <summary>
    /// 预购金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal Budget { get; set; }

    /// <summary>
    /// 实际金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// 地点
    /// </summary>
    [Column(StringLength = 200)]
    public string Address { get; set; }

    /// <summary>
    /// 地点:省
    /// </summary>
    [Column(StringLength = 50)]
    public string Province { get; set; }

    /// <summary>
    /// 地点:市
    /// </summary>
    [Column(StringLength = 50)]
    public string City { get; set; }

    /// <summary>
    /// 地点:区/县
    /// </summary>
    [Column(StringLength = 50)]
    public string District { get; set; }

    /// <summary>
    /// 地点:街道/镇
    /// </summary>
    [Column(StringLength = 70)]
    public string Street { get; set; }

    /// <summary>
    /// 记录日期：年
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 记录日期：月
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// 记录日期：日
    /// </summary>
    public int Day { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    [Column(StringLength = 10)]
    public DateTime Time { get; set; }

    /// <summary>
    /// 状态 0:正常；1：已购买
    /// </summary>
    public int Status { get; set; }

}
