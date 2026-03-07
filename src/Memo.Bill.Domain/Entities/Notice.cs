namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 公告表
/// </summary>
[Table(Name = "notice")]
public class Notice: BaseAuditEntity
{
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
