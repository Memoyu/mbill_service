using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Ledgers.Common;

public record LedgerResult : LedgerBaseResult
{
    /// <summary>
    /// 支出
    /// </summary>
    public decimal Expend { get; set; }

    /// <summary>
    /// 收入
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    public List<UserBaseResult> Users { get; set; } = [];
}
