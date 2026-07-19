namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单表
/// </summary>
[Table(Name = "bill_refund")]
[Index("idx_bill_refund_bill_id", nameof(BillId), false)]
public class BillRefund : BaseAuditEntity
{
    /// <summary>
    /// 账单Id
    /// </summary>
    [Description("账单Id")]
    [Column(CanUpdate = false)]
    public long BillId { get; set; }

    /// <summary>
    /// 账户Id
    /// </summary>
    [Description("账户Id")]
    public long AccountId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [Description("金额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Description("备注")]
    [Column(StringLength = 200, IsNullable = false)]
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 日期
    /// </summary>
    [Description("日期")]
    public DateTime Date { get; set; }
}
