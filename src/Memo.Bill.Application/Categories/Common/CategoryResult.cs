namespace Memo.Bill.Application.Categories.Common;

public record CategoryResult
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public CategoryResult? Parent { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
