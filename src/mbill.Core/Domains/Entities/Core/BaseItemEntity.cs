﻿namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 字典项详情表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_item")]
[Index("index_item_on_bid", "BId", false)]
public class BaseItemEntity : FullAduitEntity
{
    public BaseItemEntity()
    {
    }

    public BaseItemEntity(string itemCode, string itemName, bool status, int? sort)
    {
        ItemCode = itemCode ?? throw new ArgumentNullException(nameof(itemCode));
        ItemName = itemName ?? throw new ArgumentNullException(nameof(itemName));
        Status = status;
        Sort = sort;
    }

    /// <summary>
    /// 字典项所属TypeBId
    /// </summary>
    public long BaseTypeBId { get; set; }

    ///// <summary>
    ///// 字典项所属TypeId
    ///// </summary>
    //public long BaseTypeId { get; set; }

    /// <summary>
    /// 字典项编码
    /// </summary>
    [Column(StringLength = 50)]
    public string ItemCode { get; set; }

    /// <summary>
    /// 字典项名称
    /// </summary>
    [Column(StringLength = 50)]
    public string ItemName { get; set; }

    /// <summary>
    /// 状态：0禁用，1启用
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 排序码
    /// </summary>
    public int? Sort { get; set; }

    public virtual BaseTypeEntity BaseType { get; set; }
}