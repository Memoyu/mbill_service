/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.WebApi.Extensions
*   文件名称 ：AutoMapperSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:23:32
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AspNetCoreRateLimit;
using mbill_service.Core.Common.Configs;
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace mbill_service.Core.Extensions.ServiceCollection
{
    /// <summary>
    /// 配置注册服务融合
    /// </summary>
    public static class FusionSetup
    {
        /// <summary>
        /// 配置注册Automapper 服务
        /// </summary>
        public static void AddAutoMapper(this IServiceCollection services)
        {
            Assembly assemblys = Assembly.Load("mbill_service.Service");
            services.AddAutoMapper(assemblys);
        }

        /// <summary>
        /// 配置注册监控
        /// </summary>
        public static void AddMiniProfilerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddMiniProfiler();
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
            });
        }

        /// <summary>
        /// 配置注册限流依赖的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIpRateLimiting(this IServiceCollection services)
        {
            //加载配置
            services.AddOptions();
            //从IpRateLimiting.json获取相应配置
            services.Configure<IpRateLimitOptions>(Appsettings.IpRateLimitingConfig);
            services.Configure<IpRateLimitPolicies>(Appsettings.IpRateLimitPoliciesConfig);
            services.AddDistributedRateLimiting();
            //配置（计数器密钥生成器）
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }

        /// <summary>
        /// 配置注册CSRedis
        /// </summary>
        public static IServiceCollection AddCsRedisCore(this IServiceCollection services)
        {
            CSRedisClient csRedisClient = new CSRedisClient(Appsettings.CsRedisCon);
            //初始化 RedisHelper
            RedisHelper.Initialization(csRedisClient);

            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            return services;
        }
        /// <summary>
        /// 配置注册跨域
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Appsettings.Cors.CorsName, builder =>
                {
                    builder
                        .WithOrigins(
                            Appsettings.Cors
                                      .CorsOrigins
                                      .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                      .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }

}
