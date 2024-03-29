﻿namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 字典类型表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_type")]
public class BaseTypeEntity : FullAduitEntity
{
    public BaseTypeEntity()
    {
    }

    public BaseTypeEntity(string typeCode, string fullName, int? sort)
    {
        TypeCode = typeCode ?? throw new ArgumentNullException(nameof(typeCode));
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Sort = sort;
    }

    /// <summary>
    /// 字典类型编码
    /// </summary>
    [Description("字典类型编码")]
    [Column(StringLength = 50)]
    public string TypeCode { get; set; }

    /// <summary>
    /// 字典类型名
    /// </summary>
    [Description("字典类型名")]
    [Column(StringLength = 50)]
    public string FullName { get; set; }

    /// <summary>
    /// 排序码
    /// </summary>
    [Description("排序码")]
    public int? Sort { get; set; }

    public virtual ICollection<BaseItemEntity> BaseItems { get; set; }
}
