namespace Memo.Bill.Application.Bills.Common;

internal record BillPageResult : PaginationResult<BillResult>
{
    public BillPageResult(IReadOnlyList<BillResult> items, long total) : base(items, total)
    {
    }

    public DateTime Date { get; set; }

    public string Expend { get; set; }

    public string Income { get; set; }
}
