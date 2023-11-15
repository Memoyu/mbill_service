namespace Mbill.Core.Domains.Entities.PreOrder;

/// <summary>
/// 预购清单实体
/// </summary>
[Table(Name = DbTablePrefix + "_pre_order")]
[Index("index_preorder_on_group_bid", "GroupBId", false)]
[Index("index_preorder_on_status", "Status", false)]
public class PreOrderEntity : FullAduitEntity
{
    /// <summary>
    /// 分组BId
    /// </summary>
    public long GroupBId { get; set; }

    /// <summary>
    /// 预购金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal PreAmount { get; set; }

    /// <summary>
    /// 实际金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal RealAmount { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 40, IsNullable = false)]
    public string Description { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    [Column(StringLength = 10, IsNullable = false)]
    public DateTime Time { get; set; }

    /// <summary>
    /// 状态 0:未购买；1：已购买
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    public string Color { get; set; }

}
