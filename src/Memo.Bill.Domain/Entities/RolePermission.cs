namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 用户角色与权限关联表
/// </summary>
[Table(Name = "role_permission")]
[Index("idx_role_permission_role_id", nameof(RoleId), false)]
[Index("idx_role_permission_permission_id", nameof(PermissionId), false)]
public class RolePermission : BaseAuditEntity
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [Description("角色Id")]
    [Column(IsNullable = false)]
    public long RoleId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
    [Description("权限Id")]
    [Column(IsNullable = false)]
    public long PermissionId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [Navigate(nameof(Role.RoleId), TempPrimary = nameof(Role))]
    public virtual Role? Role { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [Navigate(nameof(Permission.PermissionId), TempPrimary = nameof(PermissionId))]
    public virtual Permission? Permission { get; set; }
}
