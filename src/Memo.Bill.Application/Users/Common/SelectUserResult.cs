namespace Memo.Bill.Application.Users.Common;

public class SelectUserResult
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    ///  用户头像url
    /// </summary>
    public string Avatar { get; set; } = string.Empty;
}
