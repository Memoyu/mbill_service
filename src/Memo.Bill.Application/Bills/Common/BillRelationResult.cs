namespace Memo.Bill.Application.Bills.Common;

internal record BillRelationResult
{
    /// <summary>
    /// 日期内总支出
    /// </summary>
    public decimal Expend { get; set; }

    /// <summary>
    /// 日期内总收入
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// 账单
    /// </summary>
    public List<BillResult> Items { get; set; } = [];
}