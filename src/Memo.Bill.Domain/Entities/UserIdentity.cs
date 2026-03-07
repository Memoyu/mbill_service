using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 用户身份认证表
/// </summary>
[Table(Name = "user_identity")]
[Index("idx_user_identity_identity_id", nameof(IdentityId), false)]
[Index("idx_user_identity_user_id", nameof(UserId), false)]
public class UserIdentity : BaseAuditEntity
{
    /// <summary>
    /// 用户身份认证Id
    /// </summary>
    [Snowflake]
    [Description("用户身份认证Id")]
    [Column(CanUpdate = false)]
    public long IdentityId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [Description("用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 认证类型
    /// </summary>
    [Description("认证类型， Password，GitHub、QQ、WeiXin等")]
    public UserIdentityType IdentityType { get; set; }

    /// <summary>
    /// 认证者，例如 用户名,手机号，邮件等，
    /// </summary>
    [Description("认证者，例如 用户名,手机号，邮件等，")]
    [Column(StringLength = 24, IsNullable = false)]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的
    /// </summary>
    [Description("凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的")]
    [Column(StringLength = 50, IsNullable = false)]
    public string Credential { get; set; } = string.Empty;
}
