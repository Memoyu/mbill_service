using mbill.ToolKits.Qiniu;
using MongoDB.Driver;

namespace mbill.Core.Extensions.ServiceCollection;

/// <summary>
/// 配置注册服务融合
/// </summary>
public static class FusionSetup
{
    /// <summary>
    /// 配置注册Automapper 服务
    /// </summary>
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        Assembly assemblys = Assembly.Load("mbill.Service");
        return services.AddAutoMapper(assemblys);
    }

    /// <summary>
    /// 配置注册监控
    /// </summary>
    public static IServiceCollection AddMiniProfilerSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        //services.AddMiniProfiler();
        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
        });
        return services;
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
        return services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
    }
    /// <summary>
    /// 配置注册跨域
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
       return  services.AddCors(options =>
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

    /// <summary>
    /// 配置注册HttpClient
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        return services.AddHttpClient();
    }

    /// <summary>
    /// 配置注册七牛云服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddQiniuClient(this IServiceCollection services, IConfiguration configuration, string sectionName = "FileStorage:Qiniu")
    {
        if (services == null) throw new ArgumentNullException($"{nameof(services)} is not null");
        services.Configure<QiniuClientOption>(configuration.GetSection(sectionName));
        return services.AddSingleton<IQiniuClient, QiniuClient>();
    }

    /// <summary>
    /// 配置注册MongoClient
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddMongoClient(this IServiceCollection services)
    {
       return services.AddSingleton(new MongoClient(Appsettings.MongoDBCon));
    }
}
