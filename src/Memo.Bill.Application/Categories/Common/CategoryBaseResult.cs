namespace Memo.Bill.Application.Categories.Common;

internal record CategoryBaseResult
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;
}
