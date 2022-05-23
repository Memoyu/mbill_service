namespace mbill_service.Service.Core.Auth.Input;

public class AccountLoginDto : BaseLoginInput
{
    /// <summary>
    /// 登录名
    /// </summary>
    /// <example>
    /// admin
    /// </example>
    [Required(ErrorMessage = "登录名为必填项")]
    public string Username { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    /// <example>
    /// 123456
    /// </example>
    [Required(ErrorMessage = "密码为必填项")]
    public string Password { get; set; }
}
