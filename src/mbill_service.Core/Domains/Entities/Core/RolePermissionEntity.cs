namespace mbill_service.Core.Domains.Entities.Core;

/// <summary>
/// 角色权限表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_role_permission")]
public class RolePermissionEntity : Entity
{
    public RolePermissionEntity()
    {

    }

    public RolePermissionEntity(long roleId, long permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public RolePermissionEntity(long permissionId)
    {
        PermissionId = permissionId;
    }

    /// <summary>
    /// 角色id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
    public long PermissionId { get; set; }

}
