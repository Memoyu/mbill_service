using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 用户角色表
/// </summary>
[Table(Name = "role")]
[Index("idx_role_role_id", nameof(RoleId), false)]
public class Role : BaseAuditEntity
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [Snowflake]
    [Description("角色Id")]
    [Column(CanUpdate = false)]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色唯一标识字符
    /// </summary>
    [Description("角色唯一标识字符")]
    [Column(StringLength = 60, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色类型
    /// </summary>
    [Description("角色类型")]
    [Column(IsNullable = false, CanUpdate = false)]
    public RoleType Type { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    [Description("角色描述")]
    [Column(StringLength = 100, IsNullable = false)]
    public string Description { get; set; } = string.Empty;
}
