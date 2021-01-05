/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Aop.Filter
*   文件名称 ：LocalExceptionFilter.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 20:53:07
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Shared.Exceptions;
using Memoyu.Mbill.ToolKits.Base;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Memoyu.Mbill.WebApi.Aop.Filter
{
    /// <summary>
    /// 出现异常时，如KnownException业务异常，会先执行方法过滤器 （LogActionFilterAttribute）的OnActionExecuted才会执行此异常过滤器。
    /// </summary>
    public class LocalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;
        public LocalExceptionFilter(ILogger<LocalExceptionFilter> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is KnownException exception)
            {
                ServiceResult result = new ServiceResult
                {
                    Code = exception.GetErrorCode(),
                    Message = exception.Message
                };

                _logger.LogWarning(JsonConvert.SerializeObject(result));
                HandlerException(context, result, exception.GetStatusCode());
                return;
            }

            string error = "异常信息：";

            void ReadException(Exception ex)
            {
                error += $"{ex.Message} | {ex.StackTrace} | {ex.InnerException}";
                if (ex.InnerException != null)
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(context.Exception);

            _logger.LogError(error);

            ServiceResult response = new ServiceResult 
            {
                Code= ServiceResultCode.UnknownError,
                Message = _environment.IsDevelopment() ? error : "服务器正忙，请稍后再试."
            };

            HandlerException(context, response, StatusCodes.Status500InternalServerError);
        }

        private void HandlerException(ExceptionContext context, ServiceResult response, int statusCode)
        {
            context.Result = new JsonResult(response)
            {
                StatusCode = statusCode,
                ContentType = "application/json",
            };
            context.ExceptionHandled = true;
        }
    }
}
