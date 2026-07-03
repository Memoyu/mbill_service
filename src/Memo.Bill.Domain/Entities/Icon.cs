namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 图标资源表
/// </summary>
[Table(Name = "icon")]
public class Icon : BaseAuditEntity
{
    /// <summary>
    /// 目录
    /// </summary>
    [Description("目录")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Catalog { get; set; } = string.Empty;

    /// <summary>
    /// 目录名
    /// </summary>
    [Description("目录名")]
    [Column(StringLength = 20, IsNullable = false)]
    public string CatalogName { get; set; } = string.Empty;

    /// <summary>
    /// 图标URL
    /// </summary>
    [Description("URL")]
    [Column(StringLength = 100, IsNullable = false)]
    public string Url { get; set; } = string.Empty;
}
