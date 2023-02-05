namespace mbill.Service.Core.Auth.Input;

public class WxLoginInput
{
    public string OpenId { get; set; }

    public string Nickname { get; set; }

    public string AvatarUrl { get; set; }

    public (bool flag, string Msg) Valid()
    {
        if (OpenId.IsNullOrWhiteSpace()) return (false, "微信OpenId不能为空");
        if (Nickname.IsNullOrWhiteSpace()) return (false, "用户昵称不能为空");
        if (AvatarUrl.IsNullOrWhiteSpace()) return (false, "用户头像不能为空");
        return (true, string.Empty);
    }
}
