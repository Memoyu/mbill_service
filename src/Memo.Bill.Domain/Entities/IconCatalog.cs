namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 图标目录表
/// </summary>
[Table(Name = "icon_catalog")]
public class IconCatalog : BaseAuditEntity
{
    /// <summary>
    /// 目录编码
    /// </summary>
    [Description("目录编码")]
    [Column(StringLength = 30, IsNullable = false)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 目录名称
    /// </summary>
    [Description("目录名称")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Name { get; set; } = string.Empty;
}

