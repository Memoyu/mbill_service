namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 角色表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_role")]
[Index("index_role_on_bid", "BId", false)]
public class RoleEntity : FullAduitEntity
{
    /// <summary>
    /// 超级管理员
    /// </summary>
    public const string Administrator = "Administrator";

    /// <summary>
    /// 管理员
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// 普通用户
    /// </summary>
    public const string User = "User";

    public RoleEntity()
    {

    }

    public RoleEntity(string name, string info, bool isStatic, long createUserId)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Info = info ?? throw new ArgumentNullException(nameof(info));
        IsStatic = isStatic;
        CreateUserId = createUserId;
        CreateTime = DateTime.Now;
    }

    /// <summary>
    /// 角色唯一标识字符
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    [Column(StringLength = 100)]
    public string Info { get; set; }

    /// <summary>
    /// 是否是静态分组,是静态时无法删除此分组
    /// </summary>
    public bool IsStatic { get; set; } = false;

    /// <summary>
    /// 排序码，升序
    /// </summary>
    public int SortCode { get; set; }


    [Navigate(ManyToMany = typeof(UserRoleEntity))]
    public virtual ICollection<UserEntity> Users { get; set; }

    [Navigate("RoleBId")]
    public virtual ICollection<RolePermissionEntity> RolePermissions { get; set; }
}
