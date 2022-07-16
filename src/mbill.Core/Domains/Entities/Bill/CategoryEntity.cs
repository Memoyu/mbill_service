namespace mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 账单分类实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_category")]
[Index("index_category_on_sort", "Sort", false)]
[Index("index_category_on_parent_id", "ParentId", false)]
[Index("index_category_on_type", "Type", false)]
public class CategoryEntity : FullAduitEntity
{
    /// <summary>
    /// 账单分类名
    /// </summary>
    [Column(StringLength = 20, IsNullable = false)]
    public string Name { get; set; }

    /// <summary>
    /// 父级Id，默认0
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 分类类型：支出、收入
    /// </summary>
    [Column(IsNullable = false)]
    public int Type { get; set; }

    /// <summary>
    /// 预算金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    public decimal Budget { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    [Column(StringLength = 32)]
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
