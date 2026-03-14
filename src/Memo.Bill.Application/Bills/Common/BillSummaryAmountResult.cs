namespace Memo.Bill.Application.Bills.Common;

internal record BillSummaryAmountResult
{
    public BillSummaryAmountItem Summary { get; set; } = new();

    public List<BillSummaryAmountWithDateItem> Items { get; set; } = [];
}

internal record BillSummaryAmountWithDateItem(string Date) : BillSummaryAmountItem;

internal record BillSummaryAmountItem
{
    /// <summary>
    /// 收入
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// 支出
    /// </summary>
    public decimal Expend { get; set; }

    /// <summary>
    /// 平均收入
    /// </summary>
    public decimal IncomeAvg { get => field.ToRound(); set; }

    /// <summary>
    /// 平均支出
    /// </summary>
    public decimal ExpendAvg { get => field.ToRound(); set; }

    /// <summary>
    /// 结余
    /// </summary>
    public decimal Surplus { get; set; } 

    /// <summary>
    /// 最高支出
    /// </summary>
    public decimal ExpendHighest { get; set; }

    /// <summary>
    /// 最低支出
    /// </summary>
    public decimal ExpendLowst { get; set; }

    /// <summary>
    /// 最高收入
    /// </summary>
    public decimal IncomeHighest { get; set; }

    /// <summary>
    /// 最低收入
    /// </summary>
    public decimal IncomeLowst { get; set; }
}
