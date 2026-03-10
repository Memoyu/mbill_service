namespace Memo.Bill.Application.Accounts.Common;

internal record AccountBaseResult
{
    /// <summary>
    /// 账户Id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;
}
