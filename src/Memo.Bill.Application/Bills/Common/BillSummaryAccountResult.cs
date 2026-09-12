namespace Memo.Bill.Application.Bills.Common;

internal record BillSummaryAccountResult
{
    public List<BillSummaryAccountItem> Expends { get; set; } = [];

    public List<BillSummaryAccountItem> Incomes { get; set; } = [];
}

internal record BillSummaryAccountItem
{
    /// <summary>
    /// 账户Id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 账户名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 账户图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 账单笔数
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 占比
    /// </summary>
    public decimal Percent { get; set; }
}
