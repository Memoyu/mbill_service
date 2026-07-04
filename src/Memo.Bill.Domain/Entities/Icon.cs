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
    [Column(StringLength = 30, IsNullable = false)]
    public string Catalog { get; set; } = string.Empty;

    /// <summary>
    /// 域名
    /// </summary>
    [Description("域名")]
    [Column(StringLength = 50, IsNullable = false)]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// 图标路径
    /// </summary>
    [Description("图标路径")]
    [Column(StringLength = 100, IsNullable = false)]
    public string Path { get; set; } = string.Empty;
}
