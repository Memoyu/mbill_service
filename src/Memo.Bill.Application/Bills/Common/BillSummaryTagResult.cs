namespace Memo.Bill.Application.Bills.Common;

internal record BillSummaryTagResult
{
    public List<BillSummaryTagItem> Tags { get; set; } = [];
}

internal record BillSummaryTagItem
{
    /// <summary>
    /// 标签Id
    /// </summary>
    public long TagId { get; set; }

    /// <summary>
    /// 标签名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 支出
    /// </summary>
    public decimal Expend { get; set; }

    /// <summary>
    /// 收入
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// 支出笔数
    /// </summary>
    public decimal ExpendCount { get; set; }

    /// <summary>
    ///  收入笔数
    /// </summary>
    public decimal IncomeCount { get; set; }

}
