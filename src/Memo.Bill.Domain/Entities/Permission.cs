namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 权限表
/// </summary>
[Table(Name = "permission")]
[Index("idx_permission_permission_id", nameof(PermissionId), false)]
public class Permission : BaseAuditEntity
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Snowflake]
    [Description("权限Id")]
    [Column(CanUpdate = false)]
    public long PermissionId { get; set; }

    /// <summary>
    /// 权限所属模块
    /// </summary>
    [Description("权限所属模块")]
    [Column(StringLength = 50, IsNullable = false)]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// 权限所属模块
    /// </summary>
    [Description("权限所属模块")]
    [Column(StringLength = 100, IsNullable = false)]
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 权限名称
    /// </summary>
    [Description("权限名称")]
    [Column(StringLength = 60, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 权限标识
    /// </summary>
    [Description("权限标识")]
    [Column(StringLength = 100, IsNullable = false)]
    public string Signature { get; set; } = string.Empty;
}
