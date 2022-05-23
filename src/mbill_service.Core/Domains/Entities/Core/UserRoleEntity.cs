namespace mbill_service.Core.Domains.Entities.Core;

/// <summary>
/// 用户角色表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_user_role")]
public class UserRoleEntity : Entity
{
    public UserRoleEntity()
    {
    }
    public UserRoleEntity(long userId, long roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleId { get; set; }


    [Navigate("UserId")]
    public UserEntity User { get; set; }

    [Navigate("RoleId")]
    public RoleEntity Role { get; set; }

}
