namespace Memo.Bill.Domain.Enums;

/// <summary>
/// 用户认证类型
/// </summary>
public enum UserIdentityType
{
    [Description("密码")]
    Password = 0,

    [Description("微信认证")]
    WeiXin = 1,
}

/// <summary>
/// 用户状态
/// </summary>
public enum UserStatus
{
    [Description("正常")]
    Normal = 0,

    [Description("禁用")]
    Disabled = 1,
}