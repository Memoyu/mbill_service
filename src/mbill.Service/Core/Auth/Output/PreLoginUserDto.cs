namespace mbill.Service.Core.Auth.Output;

public class PreLoginUserDto
{
    public string OpenId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    public string AvatarUrl { get; set; }

    /// <summary>
    /// 用户是否已存在
    /// </summary>
    public int IsCompletedInfo { get; set; }
}
