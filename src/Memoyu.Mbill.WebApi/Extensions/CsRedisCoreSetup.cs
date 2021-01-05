/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：CsRedisCoreSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:24:15
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using CSRedis;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyu.Mbill.WebApi.Extensions
{
    /// <summary>
    /// 配置注册CSRedis
    /// </summary>
    public static class CsRedisCoreSetup
    {
        public static IServiceCollection AddCsRedisCore(this IServiceCollection services)
        {

          
            CSRedisClient csRedisClient = new CSRedisClient(AppSettings.CsRedisCon);
            //初始化 RedisHelper
            RedisHelper.Initialization(csRedisClient);

            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            return services;
        }
    }
}
