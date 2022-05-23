using System;

namespace mbill_service.Service.Core.Auth.Output;

public class UserSimpleDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// 性别，0：未知，1：男，2：女
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    public string AvatarUrl { get; set; }

    /// <summary>
    /// 最后一次登录的时间
    /// </summary>
    public DateTime LastLoginTime { get; set; }
}
