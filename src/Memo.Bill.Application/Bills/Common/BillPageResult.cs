namespace Memo.Bill.Application.Bills.Common;

internal record BillPageResult<T> : PaginationResult<T>
{
    public BillPageResult(IReadOnlyList<T> items, long total) : base(items, total)
    {
    }

    public DateTime Date { get; set; }

    public decimal Expend { get; set; }

    public decimal Income { get; set; }
}
