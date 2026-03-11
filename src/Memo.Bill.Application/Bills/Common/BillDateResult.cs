namespace Memo.Bill.Application.Bills.Common;

internal record BillDateResult
{
    public DateTime Date { get; set; }

    public string Expend { get; set; }

    public string Income { get; set; }

    public List<BillResult> Items { get; set; } = [];
}
