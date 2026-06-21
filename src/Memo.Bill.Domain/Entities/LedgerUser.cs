namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单账本表
/// </summary>
[Table(Name = "ledger_user")]
[Index("idx_ledger_user_user_id", nameof(UserId), false)]
[Index("idx_ledger_user_ledger_id", nameof(LedgerId), false)]
public class LedgerUser : BaseAuditEntity
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [Description("用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    [Description("角色Id")]
    public long LedgerId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [Navigate(nameof(User.UserId), TempPrimary = nameof(UserId))]
    public virtual User User { get; set; } = new();

    /// <summary>
    /// 角色
    /// </summary>
    [Navigate(nameof(Ledger.LedgerId), TempPrimary = nameof(LedgerId))]
    public virtual Ledger Ledger { get; set; } = new();
}
