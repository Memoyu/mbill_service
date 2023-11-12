using Mbill.Core.Domains.Common.Enums;

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
    public int Type { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 100)]
    public string Desc { get; set; }

    /// <summary>
    /// 种子数据
    /// </summary>
    [Column(DbType = "json")]
    public string Data { get; set; }
}
