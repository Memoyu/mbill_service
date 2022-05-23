namespace mbill.Core.Extensions.ServiceCollection;

/// <summary>
/// 配置注册CAP
/// </summary>
public static class CapSetup
{
    public static IServiceCollection AddCap(this IServiceCollection services)
    {

        services.AddCap(x =>
        {
            try
            {
                x.UseCapOptions();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            x.UseDashboard();
            x.FailedRetryCount = 5;
            x.FailedThresholdCallback = (type) =>
            {
                Log.Error($@"A message of type {type} failed after executing {x.FailedRetryCount} several times, requiring manual troubleshooting. Message name: {type.Message.GetName()}");
            };
        });

        return services;
    }

    /// <summary>
    /// 根据配置文件配置Cap
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private static CapOptions UseCapOptions(this CapOptions options)
    {
        var defaultStorage = Appsettings.CapDefaultStorage;
        var defaultMessageQueue = Appsettings.CapDefaultMessageQueue;

        //配置Cap默认存储类型
        if (Enum.TryParse(defaultStorage, out CapStorageTypeEnums capStorageType))
        {
            if (!Enum.IsDefined(typeof(CapStorageTypeEnums), capStorageType))//枚举中是否存在该类型定义
            {
                Log.Error($"CAP配置:DefaultStorage:{defaultStorage}无效");
            }

            switch (capStorageType)
            {
                case CapStorageTypeEnums.InMemoryStorage:
                    options.UseInMemoryStorage();
                    break;
                case CapStorageTypeEnums.Mysql:
                    options.UseMySql(opt =>
                    {
                        opt.ConnectionString = Appsettings.MySqlCon;
                        opt.TableNamePrefix = SystemConst.DbTablePrefix;
                    });
                    break;
                default:
                    break;
            }

        }
        else
        {
            Log.Error($"CAP配置:DefaultStorage:{capStorageType}配置无效，仅支持InMemoryStorage，Mysql！更多请增加引用，修改配置项代码");
        }
        //配置Cap默认消息队列
        if (Enum.TryParse(defaultMessageQueue, out CapMessageQueueTypeEnums capMessageQueueType))
        {
            if (!Enum.IsDefined(typeof(CapMessageQueueTypeEnums), capMessageQueueType))//枚举中是否存在该类型定义
            {
                Log.Error($"CAP配置:DefaultMessageQueue:{defaultMessageQueue}无效");
            }
            //IConfigurationSection configurationSection = Configuration.GetSection($"ConnectionStrings:{capMessageQueueType}");

            switch (capMessageQueueType)
            {
                case CapMessageQueueTypeEnums.InMemoryQueue:
                    options.UseInMemoryMessageQueue();
                    break;
                case CapMessageQueueTypeEnums.RabbitMQ:
                    options.UseRabbitMQ(options =>
                    {
                        options.HostName = Appsettings.CapRabbitMq.HostName;
                        options.UserName = Appsettings.CapRabbitMq.UserName;
                        options.Password = Appsettings.CapRabbitMq.Password;
                        options.VirtualHost = Appsettings.CapRabbitMq.VirtualHost;
                    });
                    break;
                default:
                    break;
            }
        }
        else
        {
            Log.Error($"CAP配置:DefaultMessageQueue:{defaultMessageQueue}无效");
        }

        return options;
    }
}
