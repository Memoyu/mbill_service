namespace Memo.Bill.Application.Accounts.Common;

public record AccountUpdateSort
{
    /// <summary>
    /// 账户Id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
