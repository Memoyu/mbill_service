using Memo.Bill.Application.Roles.Common;

namespace Memo.Bill.Application.Users.Common;

public class UserResult
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

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 最后一次登录的时间
    /// </summary>
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 累计记账天数
    /// </summary>
    public int BillDay { get; set; }

    /// <summary>
    /// 累计账单条数
    /// </summary>
    public long BillCount { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    public List<RoleListResult> Roles { get; set; } = [];
}
