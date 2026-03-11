namespace Memo.Bill.Domain.Enums;

/// <summary>
/// 服务响应Code
/// </summary>
public enum ResultCode
{
    /// <summary>
    /// 失败
    /// </summary>
    [Description("失败")]
    Failure = 0,

    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success = 1,

    /// <summary>
    /// 令牌过期
    /// </summary>
    [Description("令牌过期")]
    TokenExpired = 4010,

    /// <summary>
    /// 无效令牌
    /// </summary>
    [Description("无效令牌")]
    TokenInvalidation = 4011,

    /// <summary>
    /// 认证失败
    /// </summary>
    [Description("认证失败")]
    AuthenticationFailure = 4012,

    /// <summary>
    /// 未授权
    /// </summary>
    [Description("未授权")]
    Forbidden = 4030,
}
