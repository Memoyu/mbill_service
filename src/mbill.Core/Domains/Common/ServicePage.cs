namespace Mbill.Core.Domains.Common;


#region 分页请求入参

/// <summary>
/// 分页加排序
/// </summary>
public class PagedAndSortedRequestDto : PagingDto, ISortedResultRequest
{
    public string Sorting { get; set; }
}

/// <summary>
/// 常规分页
/// </summary>
public class PagingDto : IPagingDto
{
    /// <summary>
    /// 每页个数
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "每页个数最小为1")]
    public int Size { get; set; } = 15;
    /// <summary>
    /// 从0开始，0时取第1页，1时取第二页
    /// </summary>
    public int Page { get; set; } = 1;

    public string Sort { get; set; }
}


public interface IPagingDto : ILimitedResultRequest
{
    int Page { get; set; }
}

public interface ILimitedResultRequest
{
    int Size { get; set; }
}

public interface ISortedResultRequest
{
    /// <summary>
    /// 分页排序
    /// </summary>
    /// <example>
    /// 例子:
    /// "Name"
    /// "Name DESC"
    /// "Name ASC, Age DESC"
    /// </example>
    string Sorting { get; set; }
}

#endregion

#region 分页请求响应

public class PagedDto<T>
{
    public long Total { get; set; }
    public IReadOnlyList<T> Items { get; set; }
    public long Page { get; set; }
    public long Size { get; set; }

    public PagedDto()
    {
    }
    public PagedDto(IReadOnlyList<T> items)
    {
        Total = items.Count;
        Items = items;
    }
    public PagedDto(IReadOnlyList<T> items, long total) : this(items)
    {
        Total = total;
    }
}

#endregion
