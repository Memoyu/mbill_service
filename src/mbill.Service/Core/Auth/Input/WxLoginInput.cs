namespace mbill.Service.Core.Auth.Input;

public class WxLoginInput
{
    public string Nickname { get; set; }

    public int Gender { get; set; }

    public string Language { get; set; }

    public string City { get; set; }

    public string Province { get; set; }

    public string Country { get; set; }

    public string AvatarUrl { get; set; }

    public string Code { get; set; }


    public (bool flag, string Msg) Valid()
    {
        if (Code.IsNullOrWhiteSpace()) return (false, "微信临时授权Code不能为空");
        if (Nickname.IsNullOrWhiteSpace()) return (false, "用户昵称不能为空");
        return (true, string.Empty);
    }
}
