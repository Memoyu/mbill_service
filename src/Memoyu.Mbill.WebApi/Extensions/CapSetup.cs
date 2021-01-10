/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：CapSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:23:51
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Memoyu.Mbill.Domain.Shared.Const;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Microsoft.Extensions.DependencyInjection;
using Savorboard.CAP.InMemoryMessageQueue;
using Serilog;
using System;

namespace Memoyu.Mbill.WebApi.Extensions
{
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
        /// <param name="Configuration"></param>
        /// <returns></returns>
        private static CapOptions UseCapOptions(this CapOptions options)
        {
            var defaultStorage = AppSettings.CapDefaultStorage;
            var defaultMessageQueue = AppSettings.CapDefaultMessageQueue;

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
                        options.UseMySql(opt=> 
                        {
                            opt.ConnectionString = AppSettings.MySqlCon;
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
                            options.HostName = AppSettings.CapRabbitMq.HostName;
                            options.UserName = AppSettings.CapRabbitMq.UserName;
                            options.Password = AppSettings.CapRabbitMq.Password;
                            options.VirtualHost = AppSettings.CapRabbitMq.VirtualHost;
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
}
