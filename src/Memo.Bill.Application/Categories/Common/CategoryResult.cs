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

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 类型：0 支出，1 收入
    /// </summary>
    public int Type { get; set; }
}

internal record CategoryResult: CategoryBaseResult
{
    /// <summary>
    /// 是否默认
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}

internal record CategoryWithParentResult : CategoryResult
{
    /// <summary>
    /// 父分类
    /// </summary>
    public CategoryResult? Parent { get; set; }

}
