namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 公告表
/// </summary>
[Table(Name = "notice")]
[Description("公告")]
public class Notice: BaseAuditEntity
{
    /// <summary>
    /// 标题
    /// </summary>
    [Description("标题")]
    [Column(StringLength = 50)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    [Description("内容")]
    [Column(StringLength = 500)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 可见范围
    /// </summary>
    [Description("可见范围")]
    [Column(DbType = "json")]
    public string Range { get; set; } = "[]";
}
