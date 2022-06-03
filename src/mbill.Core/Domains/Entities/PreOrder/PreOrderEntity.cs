namespace mbill.Core.Domains.Entities.PreOrder;

/// <summary>
/// 预购清单实体
/// </summary>
[Table(Name = DbTablePrefix + "_pre_order")]
[Index("index_preorder_on_group_id", "GroupId", false)]
[Index("index_preorder_on_bill_id", "BillId", false)]
[Index("index_preorder_on_status", "Status", false)]
public class PreOrderEntity : FullAduitEntity
{
    /// <summary>
    /// 账单Id
    /// </summary>
    public long BillId { get; set; }

    /// <summary>
    /// 分组Id
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

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
