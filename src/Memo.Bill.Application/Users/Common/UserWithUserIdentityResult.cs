namespace Memo.Bill.Application.Users.Common;

public class UserWithUserIdentityResult : UserWithRoleResult
{
    /// <summary>
    /// 用户认证方式
    /// </summary>
    public UserIdentityResult UserIdentity { get; set; } = new();
}
