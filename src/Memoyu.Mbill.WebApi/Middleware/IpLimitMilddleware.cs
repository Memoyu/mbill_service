/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Middleware
*   文件名称 ：IpLimitMilddleware.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 12:12:55
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AspNetCoreRateLimit;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;

namespace Memoyu.Mbill.WebApi.Middleware
{
    public static class IpLimitMilddleware
    {
        public static void UseIpLimitMilddleware(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                var isEnabled = AppSettings.IpRateLimitEnable;
                if (isEnabled)
                {
                    app.UseIpRateLimiting();
                }
            }
            catch (Exception e)
            {
                Log.Error($"添加IP限流发生异常.\n{e.Message}");
                throw;
            }
        }
    }
}
