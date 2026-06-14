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

    [Description("QQ认证")]
    Qq = 2,

    [Description("Github认证")]
    GitHub = 3,

    [Description("Gitee认证")]
    Gitee = 4,
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