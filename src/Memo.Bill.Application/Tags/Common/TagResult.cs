namespace Memo.Bill.Application.Tags.Common;

internal record TagBaseResult
{
    /// <summary>
    /// 账户Id
    /// </summary>
    public long TagId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }
}

internal record TagResult : TagBaseResult
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}

internal record TagWithParentResult : TagResult
{
    /// <summary>
    /// 父标签
    /// </summary>
    public TagResult? Parent { get; set; }

}
