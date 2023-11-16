namespace Mbill.Core.Domains.Entities.User;

/// <summary>
/// 用户实体
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_user")]
public class UserEntity : FullAduitEntity
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Description("用户名")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Username { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Description("昵称")]
    [Column(StringLength = 20, IsNullable = false)]
    public string Nickname { get; set; }

    /// <summary>
    /// 性别，0：未知，1：男，2：女
    /// </summary>
    [Description("性别，0：未知，1：男，2：女")]
    public int Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Description("邮箱")]
    [Column(StringLength = 60)]
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [Description("电话")]
    [Column(StringLength = 20)]
    public string Phone { get; set; }

    /// <summary>
    /// 省
    /// </summary>
    [Description("省")]
    [Column(StringLength = 20)]
    public string Province { get; set; }

    /// <summary>
    /// 市
    /// </summary>
    [Description("市")]
    [Column(StringLength = 20)]
    public string City { get; set; }

    /// <summary>
    /// 区
    /// </summary>
    [Description("区")]
    [Column(StringLength = 20)]
    public string District { get; set; }

    /// <summary>
    /// 街道
    /// </summary>
    [Description("街道")]
    [Column(StringLength = 50)]
    public string Street { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    [Description("头像地址")]
    [Column(StringLength = 200)]
    public string AvatarUrl { get; set; }

    /// <summary>
    /// 最后一次登录的时间
    /// </summary>
    [Description("最后一次登录的时间")]
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// JWT 登录，保存生成的随机token值。
    /// </summary>
    [Description("JWT 登录，保存生成的随机token值。")]
    [Column(StringLength = 200)]
    public string RefreshToken { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Description("是否启用")]
    public bool IsEnable { get; set; } = true;

    /// <summary>
    /// 是否初始化用户数据
    /// </summary>
    [Description("是否初始化用户数据")]
    public bool IsInit { get; set; }

    [Navigate(nameof(UserRoleEntity.UserBId), TempPrimary = nameof(BId))]
    public virtual IEnumerable<UserRoleEntity> UserRoles { get; set; }

    /// <summary>
    /// 登录后用户状态变化
    /// </summary>
    /// <param name="refreshToken"></param>
    public void ChangeLoginStatus(string refreshToken)
    {
        LastLoginTime = DateTime.Now;
        RefreshToken = refreshToken;
    }

}
