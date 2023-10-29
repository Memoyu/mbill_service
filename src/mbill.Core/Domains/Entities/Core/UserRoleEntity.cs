namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 用户角色表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_user_role")]
[Index("index_user_role_on_bid", "BId", false)]
[Index("index_user_role_on_user_bid", "UserBId", false)]
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
    public long UserBId { get; set; }

    ///// <summary>
    ///// 用户Id
    ///// </summary>
    //public long UserId { get; set; }

    /// <summary>
    /// 角色BId
    /// </summary>
    public long RoleBId { get; set; }

    ///// <summary>
    ///// 角色Id
    ///// </summary>
    //public long RoleId { get; set; }


    [Navigate("UserBId")]
    public UserEntity User { get; set; }

    [Navigate("RoleBId")]
    public RoleEntity Role { get; set; }

}
