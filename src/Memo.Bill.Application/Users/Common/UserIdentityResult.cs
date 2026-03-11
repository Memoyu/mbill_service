using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Application.Users.Common;

public record UserIdentityResult
{
    /// <summary>
    /// 认证类型
    /// </summary>
    public UserIdentityType IdentityType { get; set; }

    /// <summary>
    /// 认证者，例如 用户名,手机号，邮件等，
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的
    /// </summary>
    public string Credential { get; set; } = string.Empty;
}
