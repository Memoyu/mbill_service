namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 用户身份认证登录表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_user_identity")]
public class UserIdentityEntity : FullAduitEntity
{
    public const string GitHub = "GitHub";
    public const string Password = "Password";
    public const string QQ = "QQ";
    public const string Gitee = "Gitee";
    public const string WeiXin = "WeiXin";

    public UserIdentityEntity()
    {
    }

    public UserIdentityEntity(string identityType, string identifier, string credential, DateTime createTime)
    {
        IdentityType = identityType ?? throw new ArgumentNullException(nameof(identityType));
        Identifier = identifier;
        Credential = credential ?? throw new ArgumentNullException(nameof(credential));
        CreateTime = createTime;
    }

    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///认证类型， Password，GitHub、QQ、WeiXin等
    /// </summary>
    [Column(StringLength = 20)]
    public string IdentityType { get; set; }

    /// <summary>
    /// 认证者，例如 用户名,手机号，邮件等，
    /// </summary>
    [Column(StringLength = 24)]
    public string Identifier { get; set; }

    /// <summary>
    ///  凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的
    /// </summary>
    [Column(StringLength = 50)]
    public string Credential { get; set; }

    /// <summary>
    /// 扩展属性
    /// </summary>
    public string ExtraProperties { get; set; }
}
