/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：IpRateLimitingSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:24:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AspNetCoreRateLimit;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyu.Mbill.WebApi.Extensions
{
    /// <summary>
    /// Ip限流服务配置注册
    /// </summary>
    public static class IpRateLimitingSetup
    {
        /// <summary>
        /// 配置限流依赖的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIpRateLimiting(this IServiceCollection services)
        {
            //加载配置
            services.AddOptions();
            //从IpRateLimiting.json获取相应配置
            services.Configure<IpRateLimitOptions>(AppSettings.IpRateLimitingConfig);
            services.Configure<IpRateLimitPolicies>(AppSettings.IpRateLimitPoliciesConfig);
            //注入计数器和规则存储
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            //配置（计数器密钥生成器）
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }

    }
}
