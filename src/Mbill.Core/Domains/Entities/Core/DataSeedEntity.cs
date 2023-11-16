namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 种子数据表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_data_seed")]
public class DataSeedEntity : FullAduitEntity
{
    /// <summary>
    /// 数据类型
    /// </summary>
    [Description("数据类型")]
    public int Type { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Description("描述")]
    [Column(StringLength = 100)]
    public string Desc { get; set; }

    /// <summary>
    /// 种子数据
    /// </summary>
    [Description("种子数据")]
    [Column(DbType = "json")]
    public string Data { get; set; }
}
