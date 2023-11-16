using Mbill.Core.Common;
using Mbill.Core.Interface.IRepositories.Core;
using Serilog;
using Serilog.Events;

namespace Mbill.Core.AOP.Middleware;

/// <summary>
/// 中间件：记录请求IP
/// </summary>
public class IPLogMilddleware
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    //***************请求代理需要先注入IHttpContextAccessor，否者报错********************//
    private readonly RequestDelegate _requestDelegate;
    private readonly IIPLogRepo _ipLogRepo;

    public IPLogMilddleware(ILoggerFactory loggerFactory,
        RequestDelegate requestDelegate,
        IIPLogRepo ipLogRepo)
    {
        _logger = loggerFactory.CreateLogger<IPLogMilddleware>();
        _requestDelegate = requestDelegate;
        _ipLogRepo = ipLogRepo;
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
                    var visitEntity = new IPLogEntity
                    {
                        BId = SnowFlake.NextId(),
                        Ip = GetClientIp(context),
                        Path = request.Path.ToString().Trim().TrimEnd('/').ToLower(),
                        VisitTime = DateTime.Now,
                    };

                    if (!string.IsNullOrWhiteSpace(visitEntity.Ip))
                    {
                        _ = _ipLogRepo.InsertAsync(visitEntity);
                    }

                    await _requestDelegate(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "IP访问记录中间件异常");
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
            if (context.Connection.RemoteIpAddress != null) ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        return ip;
    }
}
