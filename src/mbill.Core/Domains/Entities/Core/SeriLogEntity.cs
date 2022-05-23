namespace mbill.Core.Domains.Entities.Core;

/// <summary>
/// 日志表
/// </summary>
[Table(DisableSyncStructure = true, Name = SystemConst.DbTablePrefix + "_serilog")]
public class SeriLogEntity
{
    public long Id { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string Exception { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 信息模板
    /// </summary>
    public string MessageTemplate { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    public string Properties { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }

}
