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
    /// 颜色
    /// </summary>
    [Description("颜色")]
    public int Color { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Sort { get; set; }
}
