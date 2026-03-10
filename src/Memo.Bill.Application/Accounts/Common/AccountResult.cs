namespace Memo.Bill.Application.Accounts.Common;

internal record AccountResult : AccountBaseResult
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public AccountResult? Parent { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
