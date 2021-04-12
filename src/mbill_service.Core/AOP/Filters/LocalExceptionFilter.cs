using mbill_service.Core.Domains.Common;
using mbill_service.Core.Domains.Common.Enums.Base;
using mbill_service.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace mbill_service.Core.AOP.Filters
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
                Code = ServiceResultCode.UnknownError,
                Message = _environment.IsDevelopment() ? error : "服务器正忙，请稍后再试."
            };

            HandlerException(context, response, StatusCodes.Status500InternalServerError);
        }

        private void HandlerException(ExceptionContext context, ServiceResult response, int statusCode)
        {
            var set = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(response, set),
                StatusCode = statusCode,
                ContentType = "application/json",
            };
            context.ExceptionHandled = true;
        }
    }
}
