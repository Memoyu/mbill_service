namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单退款表
/// </summary>
[Table(Name = "bill_refund")]
[Index("idx_bill_refund_refund_id", nameof(RefundId), false)]
[Index("idx_bill_refund_bill_id", nameof(BillId), false)]
public class BillRefund : BaseAuditEntity
{
    /// <summary>
    /// 退款Id
    /// </summary>
    [Snowflake]
    [Description("退款Id")]
    [Column(CanUpdate = false)]
    public long RefundId { get; set; }

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
    /// 退款金额
    /// </summary>
    [Description("退款金额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 账单金额
    /// </summary>
    [Description("账单金额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal AmountBefore { get; set; }

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

    /// <summary>
    /// 账单账户
    /// </summary>
    [Navigate(nameof(AccountId), TempPrimary = nameof(Account.AccountId))]
    public virtual Account Account { get; set; } = new();
}
