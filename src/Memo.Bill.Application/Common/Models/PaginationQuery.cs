namespace Memo.Bill.Application.Common.Models;

public record PaginationQuery
{
    /// <summary>
    /// 每页条数
    /// </summary>
    public int Size { get; set; } = 15;

    /// <summary>
    /// 页数
    /// </summary>
    public int Page { get; set; } = 1;

    public string Sort { get; set; } = string.Empty;
}
