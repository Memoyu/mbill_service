namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单标签表
/// </summary>
[Table(Name = "tag")]
[Index("idx_tag_id", nameof(TagId), false)]
[Index("idx_tag_parent_id", nameof(ParentId), false)]
public class Tag : BaseAuditEntity
{
    /// <summary>
    /// 标签Id
    /// </summary>
    [Snowflake]
    [Description("标签Id")]
    [Column(CanUpdate = false)]
    public long TagId { get; set; }

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
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Sort { get; set; }
}
