namespace Memo.Bill.Application.Bills.Common;

internal record BillPageGroupByMonth
{
    /// <summary>
    /// 月
    /// </summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>
    /// 账单
    /// </summary>
    public List<BillResult> Items { get; set; } = [];
}

