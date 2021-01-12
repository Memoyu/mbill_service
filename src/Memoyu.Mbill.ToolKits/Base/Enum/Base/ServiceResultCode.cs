/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Base.Enum.Base
*   文件名称 ：ServiceResultCode.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-28 0:34:42
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using System.ComponentModel;

namespace Memoyu.Mbill.ToolKits.Base.Enum.Base
{
    /// <summary>
    /// 服务层响应枚举码
    /// </summary>
    public enum ServiceResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1,

        /// <summary>
        /// 认证失败
        /// </summary>
        AuthenticationFailed = 10000,

        /// <summary>
        /// 无权限
        /// </summary>
        NoPermission = 10001,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 1007,

        /// <summary>
        /// 资源不存在
        /// </summary>
        NotFound = 10020,

        #region Parameter
        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParameterError = 10030,

        /// <summary>
        /// 字段重复
        /// </summary>
        RepeatField = 10060, 
        #endregion


        #region Token
        /// <summary>
        /// 令牌失效
        /// </summary>
        [Description("令牌失效")]
        TokenInvalidation = 10040,

        /// <summary>
        /// 令牌过期
        /// </summary>
        TokenExpired = 10050,

        /// <summary>
        /// refreshToken异常
        /// </summary>
        RefreshTokenError = 10100, 
        #endregion

    }
}
