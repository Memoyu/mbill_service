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
    public long RoleBId { get; set; }

    /// <summary>
    /// 权限BId
    /// </summary>
    public long PermissionBId { get; set; }
}
