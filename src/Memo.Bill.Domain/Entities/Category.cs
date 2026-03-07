namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单分类表
/// </summary>
[Table(Name = "category")]
[Index("idx_category_category_id", nameof(CategoryId), false)]
[Index("idx_category_parent_id", nameof(ParentId), false)]
public class Category : BaseAuditEntity
{
    /// <summary>
    /// 分类Id
    /// </summary>
    [Snowflake]
    [Description("分类Id")]
    [Column(CanUpdate = false)]
    public long CategoryId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [Description("父级Id")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Description("名称")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 类型：0 支出，1 收入
    /// </summary>
    [Description("类型：0-支出，1-收入")]
    [Column(IsNullable = false)]
    public int Type { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Description("图标")]
    [Column(StringLength = 100)]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 是否默认
    /// </summary>
    [Description("是否默认")]
    [Column(IsNullable = false)]
    public bool IsDefault { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Sort { get; set; }
}
