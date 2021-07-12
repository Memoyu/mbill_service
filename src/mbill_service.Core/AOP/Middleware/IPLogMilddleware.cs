using mbill_service.Core.Common.Configs;
using mbill_service.Core.Domains.Common;
using mbill_service.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace mbill_service.Core.AOP.Middleware
{
    /// <summary>
    /// 中间件：记录请求IP
    /// </summary>
    public class IPLogMilddleware
    {
        //***************请求代理需要先注入IHttpContextAccessor，否者报错********************//
        private readonly RequestDelegate _requestDelegate;
        private static readonly ILogger IpLog = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .WriteTo.File(Path.Combine($"Logs/ip_log/", $"ip_log.log"), rollingInterval: RollingInterval.Infinite, outputTemplate: "{Message}{NewLine}{Exception}")
            .CreateLogger();

        public IPLogMilddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isEnabled = Appsettings.IpLogEnable;

            if (isEnabled)
            {
                // 过滤，只有接口
                if (context.Request.Path.Value != null && context.Request.Path.Value.Contains("api"))
                {
                    context.Request.EnableBuffering();

                    try
                    {
                        // 存储请求数据
                        var request = context.Request;
                        var requestInfo = new RequestInfo()
                        {
                            Ip = GetClientIp(context),
                            Url = request.Path.ToString().Trim().TrimEnd('/').ToLower(),
                            Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            Week = GetWeek(),
                        }.ToJson();

                        if (!string.IsNullOrEmpty(requestInfo))
                        {
                            // 自定义log输出
                            Parallel.For(0, 1, e =>
                            {
                                WriteLog(new string[] { requestInfo + "," }, false);
                            });

                            request.Body.Position = 0;
                        }

                        await _requestDelegate(context);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    await _requestDelegate(context);
                }
            }
            else
            {
                await _requestDelegate(context);
            }
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetClientIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                if (context.Connection.RemoteIpAddress != null) ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取当前星期
        /// </summary>
        /// <returns></returns>
        private string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="messages">写入信息</param>
        /// <param name="isHeader">是否加头部分割线</param>
        private static void WriteLog(string[] messages, bool isHeader = true)
        {
            var now = DateTime.Now;
            string logContent = String.Join("\r\n", messages);
            if (isHeader)
            {
                logContent = (
                    "--------------------------------\r\n" +
                    now + "|\r\n" +
                    String.Join("\r\n", messages) + "\r\n"
                );
            }
            IpLog.Information(logContent);
        }
    }
}
