namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单关联标签表
/// </summary>
[Table(Name = "bill_tag")]
[Index("idx_bill_tag_bill_id", nameof(BillId), false)]
[Index("idx_bill_tag_tag_id", nameof(TagId), false)]
public class BillTag : BaseAuditEntity
{
    /// <summary>
    /// 账单Id
    /// </summary>
    [Description("账单Id")]
    public long BillId { get; set; }

    /// <summary>
    /// 标签Id
    /// </summary>
    [Description("标签Id")]
    public long TagId { get; set; }
}
