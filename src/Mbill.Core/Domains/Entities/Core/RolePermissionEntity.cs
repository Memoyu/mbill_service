namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 角色权限表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_role_permission")]
public class RolePermissionEntity : Entity
{
    /// <summary>
    /// 角色BId
    /// </summary>
    [Description("角色BId")]
    public long RoleBId { get; set; }

    /// <summary>
    /// 权限BId
    /// </summary>
    [Description("权限BId")]
    public long PermissionBId { get; set; }
}
