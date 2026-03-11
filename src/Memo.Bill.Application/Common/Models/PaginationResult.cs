namespace Memo.Bill.Application.Common.Models;
public record PaginationResult<T>
{
    public PaginationResult(IReadOnlyList<T> items)
    {
        Items = items;
    }
    public PaginationResult(IReadOnlyList<T> items, long total) : this(items)
    { 
        Total = total;
    }

    public long Total { get; set; }
    public IReadOnlyList<T> Items { get; set; }
    //public long Page { get; set; }
    //public long Size { get; set; }
}
