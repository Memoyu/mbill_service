using EasyCaching.FreeRedis;
using EasyCaching.Serialization.SystemTextJson.Configurations;
using FreeRedis;
using Mapster;
using MapsterMapper;
using Mbill.ToolKits.Qiniu;
using MongoDB.Driver;

namespace Mbill.Core.Extensions.ServiceCollection;

/// <summary>
/// 配置注册服务融合
/// </summary>
public static class FusionSetup
{
    /// <summary>
    /// 配置注册Mapster服务
    /// </summary>
    public static IServiceCollection AddMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        // 获取全局映射配置
        var config = TypeAdapterConfig.GlobalSettings;

        // 扫描所有继承  IRegister 接口的对象映射配置
        if (assemblies != null && assemblies.Length > 0) config.Scan(assemblies);

        // 配置默认全局映射（支持覆盖）
        config.Default
              .NameMatchingStrategy(NameMatchingStrategy.Flexible)
              .PreserveReference(true);

        // 配置默认全局映射（忽略大小写敏感）
        config.Default
              .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
              .PreserveReference(true);

        // 配置支持依赖注入
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
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
        services.AddInMemoryRateLimiting();

        //配置（计数器密钥生成器）
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        return services;
    }

    /// <summary>
    /// 配置注册EasyCaching
    /// </summary>
    public static IServiceCollection AddEasyCaching(this IServiceCollection services)
    {
        services.AddEasyCaching(ecops =>
             ecops.UseFreeRedis(frops =>
             {
                 frops.DBConfig = new FreeRedisDBOptions
                 {
                     ConnectionStrings = new List<ConnectionStringBuilder>
                     {
                        Appsettings.RedisCon
                     }
                 };
                 frops.SerializerName = "json";
             }).UseRedisLock().WithSystemTextJson());
        return services;
    }

    /// <summary>
    /// 配置注册跨域
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        return services.AddCors(options =>
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
