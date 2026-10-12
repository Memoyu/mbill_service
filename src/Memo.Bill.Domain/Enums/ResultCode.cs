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
    /// 未授权
    /// </summary>
    [Description("未授权")]
    Unauthorized = 401,

    /// <summary>
    /// 禁止访问
    /// </summary>
    [Description("禁止访问")]
    Forbidden = 403,
}
