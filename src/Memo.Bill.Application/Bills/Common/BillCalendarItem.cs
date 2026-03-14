namespace Memo.Bill.Application.Bills.Common;

internal record BillCalendarItem
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 账单数量
    /// </summary>
    public int Count { get; set; }
}
