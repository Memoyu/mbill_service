/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Exceptions
*   文件名称 ：KnownException.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 21:02:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using System;

namespace Memoyu.Mbill.Application.Contracts.Exceptions
{
    [Serializable]
    public class KnownException : ApplicationException
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private readonly int _statusCode;
        /// <summary>
        /// 错误码，当为1时，代表正常
        /// </summary>
        private readonly ServiceResultCode _errorCode;
        /// <summary>
        /// 
        /// </summary>
        public KnownException() : base("服务器繁忙，请稍后再试!")
        {
            _errorCode = ServiceResultCode.Failed;
            _statusCode = 400;
        }

        public KnownException(string message = "服务器繁忙，请稍后再试!", ServiceResultCode errorCode = ServiceResultCode.Failed, int statusCode = 200) : base(message)
        {
            this._errorCode = errorCode;
            _statusCode = statusCode;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetStatusCode()
        {
            return _statusCode;
        }

        public ServiceResultCode GetErrorCode()
        {
            return _errorCode;
        }
    }
}
