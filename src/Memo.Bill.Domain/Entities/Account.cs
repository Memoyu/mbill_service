namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单账户表
/// </summary>
[Table(Name = "account")]
[Index("idx_account_asset_id", nameof(AccountId), false)]
[Index("idx_account_parent_id", nameof(ParentId), false)]
public class Account : BaseAuditEntity
{
    /// <summary>
    /// 账户Id
    /// </summary>
    [Snowflake]
    [Description("账户Id")]
    [Column(CanUpdate = false)]
    public long AccountId { get; set; }

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
    /// 图标
    /// </summary>
    [Description("图标")]
    [Column(StringLength = 100, IsNullable = false)]
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
