namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 关联账单表
/// </summary>
[Table(Name = "bill_relation")]
[Index("idx_bill_relation_bill_id", nameof(BillId), false)]
[Index("idx_bill_relation_relation_id", nameof(RelationId), false)]
public class BillRelation : BaseAuditEntity
{
    /// <summary>
    /// 账单Id
    /// </summary>
    [Description("账单Id")]
    public long BillId { get; set; }

    /// <summary>
    /// 关联账单Id
    /// </summary>
    [Description("关联账单Id")]
    public long RelationId { get; set; }

    /// <summary>
    /// 关联账单
    /// </summary>
    [Navigate(nameof(Billing.BillId), TempPrimary = nameof(RelationId))]
    public virtual Billing RelatedBill { get; set; } = new();
}
