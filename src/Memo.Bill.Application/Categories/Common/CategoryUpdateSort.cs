namespace Memo.Bill.Application.Categories.Common;

public record CategoryUpdateSort
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
