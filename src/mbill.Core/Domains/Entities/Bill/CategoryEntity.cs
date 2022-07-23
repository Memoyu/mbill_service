namespace mbill.Core.Domains.Entities.Bill;

/// <summary>
/// 账单分类实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_category")]
[Index("index_category_on_parent_id", "ParentId", false)]
[Index("index_category_on_type", "Type", false)]
public class CategoryEntity : FullAduitEntity
{
    /// <summary>
    /// 分类Id
    /// </summary>
    [Column(IsNullable = false)]
    [Description("分类Id")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 账单分类名
    /// </summary>
    [Column(StringLength = 20, IsNullable = false)]
    [Description("分类名称")]
    public string Name { get; set; }

    /// <summary>
    /// 父级Id，默认0
    /// </summary>
    [Description("父级Id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 分类类型：0 支出，1 收入
    /// </summary>
    [Column(IsNullable = false)]
    [Description("分类类型：0 支出，1 收入")]
    public int Type { get; set; }

    /// <summary>
    /// 预算金额
    /// </summary>
    [Column(Precision = 12, Scale = 2)]
    [Description("预算金额")]
    public decimal Budget { get; set; }

    /// <summary>
    /// 图标地址
    /// </summary>
    [Column(StringLength = 100)]
    [Description("图标地址")]
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Sort { get; set; }

}
