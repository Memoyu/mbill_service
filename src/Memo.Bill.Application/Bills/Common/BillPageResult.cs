namespace Memo.Bill.Application.Bills.Common;

internal record BillPageResult<T> : PaginationResult<T>
{
    public BillPageResult(IReadOnlyList<T> items, long total) : base(items, total)
    {
    }
}
