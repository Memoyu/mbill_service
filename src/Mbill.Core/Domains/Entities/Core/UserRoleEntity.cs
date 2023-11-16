namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 用户角色表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_user_role")]
[Index("index_user_role_on_user_bid", nameof(UserBId), false)]
public class UserRoleEntity : Entity
{
    public UserRoleEntity()
    {
    }
    public UserRoleEntity(long userBId, long roleBId)
    {
        UserBId = userBId;
        RoleBId = roleBId;
    }

    /// <summary>
    /// 用户BId
    /// </summary>
    [Description("用户BId")]
    public long UserBId { get; set; }

    /// <summary>
    /// 角色BId
    /// </summary>
    [Description("角色BId")]
    public long RoleBId { get; set; }

    [Navigate(nameof(UserBId), TempPrimary = nameof(UserEntity.BId))]
    public virtual UserEntity User { get; set; }

    [Navigate(nameof(RoleBId), TempPrimary = nameof(RoleEntity.BId))]
    public virtual RoleEntity Role { get; set; }

}
