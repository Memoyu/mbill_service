namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单账本表
/// </summary>
[Table(Name = "ledger_user")]
[Index("idx_ledger_user_ledger_id", nameof(LedgerId), false)]
[Index("idx_ledger_user_user_id", nameof(UserId), false)]
public class LedgerUser : BaseAuditEntity
{
    /// <summary>
    /// 账本Id
    /// </summary>
    [Description("账本Id")]
    public long LedgerId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [Description("用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [Navigate(nameof(User.UserId), TempPrimary = nameof(UserId))]
    public virtual User User { get; set; } = new();

    /// <summary>
    /// 账本
    /// </summary>
    [Navigate(nameof(Ledger.LedgerId), TempPrimary = nameof(LedgerId))]
    public virtual Ledger Ledger { get; set; } = new();
}
