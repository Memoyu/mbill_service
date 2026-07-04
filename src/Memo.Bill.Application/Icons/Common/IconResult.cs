namespace Memo.Bill.Application.Icons.Common;

public record IconResult
{
    /// <summary>
    /// 目录
    /// </summary>
    public string Catalog { get; set; } = string.Empty;

    /// <summary>
    /// 图标路径
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 图标URL
    /// </summary>
    public string Url { get; set; } = string.Empty;
}

public record IconCatalogResult
{
    /// <summary>
    /// 目录编码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 目录名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图标集合
    /// </summary>
    public List<IconResult> Icons { get; set; } = [];
}