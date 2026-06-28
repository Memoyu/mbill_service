namespace Memo.Bill.Application.Ledgers.Common;

public record LedgerBaseResult
{
    /// <summary>
    /// 账本Id
    /// </summary>
    public long LedgerId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 颜色
    /// </summary>
    public int Color { get; set; }

    /// <summary>
    /// 默认账本
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
