namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 角色权限表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_role_permission")]
[Index("index_role_permission_on_bid", "BId", false)]
public class RolePermissionEntity : Entity
{
    public RolePermissionEntity()
    {

    }

    public RolePermissionEntity(long roleBId, long permissionBId)
    {
        RoleBId = roleBId;
        PermissionBId = permissionBId;
    }

    public RolePermissionEntity(long permissionBId)
    {
        PermissionBId = permissionBId;
    }

    /// <summary>
    /// 角色BId
    /// </summary>
    public long RoleBId { get; set; }

    /// <summary>
    /// 角色id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 权限BId
    /// </summary>
    public long PermissionBId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
    public long PermissionId { get; set; }

}
