namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 公告表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_notice")]
public class NoticeEntity : FullAduitEntity
{
    /// <summary>
    /// 内容
    /// </summary>
    [Description("内容")]
    [Column(StringLength = 500)]
    public string Content { get; set; }

    /// <summary>
    /// 可见范围
    /// </summary>
    [Description("可见范围")]
    [Column(DbType = "json")]
    public string Range { get; set; }

}
