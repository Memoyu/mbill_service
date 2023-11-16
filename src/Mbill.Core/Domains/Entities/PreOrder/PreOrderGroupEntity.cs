namespace Mbill.Core.Domains.Entities.PreOrder;

/// <summary>
/// 预购清单分组实体
/// </summary>
[Table(Name = DbTablePrefix + "_pre_order_group")]
[Index("index_preorder_group_on_bill_bid", nameof(BillBId), false)]
public class PreOrderGroupEntity : FullAduitEntity
{
    /// <summary>
    /// 账单BId
    /// </summary>
    [Description("账单BId")]
    public long BillBId { get; set; }

    /// <summary>
    /// 分组名
    /// </summary>
    [Description("分组名")]
    [Column(StringLength = 10)]
    public string Name { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Description("说明")]
    [Column(StringLength = 40)]
    public string Description { get; set; }

}
