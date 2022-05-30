namespace mbill.Core.Domains.Entities.PreOrder;

/// <summary>
/// 预购清单分组实体
/// </summary>
[Table(Name = DbTablePrefix + "_pre_order_group")]
[Index("index_preorder_group_on_category_id", "CategoryId", false)]
[Index("index_preorder_group_on_bill_id", "BillId", false)]
public class PreOrderGroupEntity : FullAduitEntity
{
    /// <summary>
    /// 分组名
    /// </summary>
    [Column(StringLength = 20)]
    public string Name { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

}
