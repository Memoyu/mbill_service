namespace mbill.Core.Domains.Entities.PreOrder;

/// <summary>
/// 预购清单实体
/// </summary>
[Table(Name = DbTablePrefix + "_pre_order")]
[Index("index_preorder_on_category_id", "CategoryId", false)]
[Index("index_preorder_on_bill_id", "BillId", false)]
public class PreOrderEntity : FullAduitEntity
{
    /// <summary>
    /// 账单Id
    /// </summary>
    public long BillId { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

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
