namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单账本表
/// </summary>
[Table(Name = "ledger")]
[Index("idx_ledger_id", nameof(LedgerId), false)]
public class Ledger : BaseAuditEntity
{
    /// <summary>
    /// 账本Id
    /// </summary>
    [Snowflake]
    [Description("账本Id")]
    [Column(CanUpdate = false)]
    public long LedgerId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Description("名称")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 默认账本
    /// </summary>
    public bool Default { get; set; }
}
