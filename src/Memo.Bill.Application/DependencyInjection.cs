using Memo.Bill.Application.Common.Attributes;
using Memo.Bill.Application.Common.Models.Settings;
using Memo.Bill.Application.Common.Services.Background;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Xml.Linq;

namespace Memo.Bill.Application;

/// <summary>
/// Application 依赖注入
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 配置雪花ID生成
        SnowFlakeUtil.Init();

        // 注册Http请求服务
        services.AddHttpClient();

        // 注册服务配置
        services.Configure<AppSettings>(configuration.GetSection(AppConst.AppSettingSection));
        services.Configure<AuthorizationSettings>(configuration.GetSection(AppConst.AuthorizationSection));

        // 注册Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // 注册消息组件
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(GlobalExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        // 注册实体映射组件
        services.AddMapper();

        // 注册app服务
        services.AddAppServices();

        // 注册后台任务
        services.AddHostedServices();

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        // 获取全局映射配置
        var config = TypeAdapterConfig.GlobalSettings;

        // 扫描 IRegister 接口的对象映射配置
        config.Scan(Assembly.GetExecutingAssembly());

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

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        // 添加GitHub Repo拉取 定时任务
        services.AddHostedService<DemoTaskService>();

        return services;
    }

    private static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        services.AddAssemblyServices(assemblyName ?? string.Empty);

        return services;
    }

    /// <summary>
    /// 批量注册AppService
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddAssemblyServices(this IServiceCollection services, string assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName)) throw new ArgumentNullException("注册AppService异常，传入的assemblyName为空");

        // 获取需要注入的类
        List<Type> types = Assembly.Load(assemblyName).GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract&& t.GetCustomAttributes(typeof(AppServiceAttribute), false).Length > 0)
            .ToList();

        // 注册类型
        types.ForEach(impl =>
        {
            //获取该类所继承的所有接口
            Type[] interfaces = impl.GetInterfaces();
            //获取该类注入的生命周期
            var lifetime = impl.GetCustomAttribute<AppServiceAttribute>()!.ServiceLifeType;

            interfaces.ToList().ForEach(i =>
            {
                switch (lifetime)
                {
                    case ServiceLifeType.Singleton:
                        services.AddSingleton(i, impl);
                        break;
                    case ServiceLifeType.Scoped:
                        services.AddScoped(i, impl);
                        break;
                    case ServiceLifeType.Transient:
                        services.AddTransient(i, impl);
                        break;
                }
            });
        });
    }
}
