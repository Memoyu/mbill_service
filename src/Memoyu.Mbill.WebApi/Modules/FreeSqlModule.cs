/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Modules
*   文件名称 ：FreeSqlModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 15:28:40
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using FreeSql;
using FreeSql.Internal;
using Memoyu.Mbill.Application.Contracts.Base;
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Data;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Memoyu.Mbill.Domain.Shared.Extensions;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading;

namespace Memoyu.Mbill.WebApi.Modules
{
    public class FreeSqlModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            IFreeSql fsql = new FreeSqlBuilder()
              .UseConnectionString(AppSettings.Configuration)
              .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
              .UseAutoSyncStructure(true)
              .UseNoneCommandParameter(true)
              .UseMonitorCommand(cmd =>
              {
                  Trace.WriteLine(cmd.CommandText + ";");
              }
              )
              .CreateDatabaseIfNotExists()
              .Build()
              .SetDbContextOptions(opt =>
              {
                  opt.EnableAddOrUpdateNavigateList = true;
                  opt.OnEntityChange = rep =>
                  {
                      //进行审计
                  };
              });//联级保存功能开启（默认为关闭）

            fsql.Aop.CurdAfter += (s, e) =>
            {
                Log.Debug($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}: FullName:{e.EntityType.FullName}" + $" ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");

                if (e.ElapsedMilliseconds > 200)
                {
                    //记录日志
                    //发送短信给负责人
                }
            };
            builder.RegisterInstance(fsql).SingleInstance();//FreeSql注册为单例
            builder.RegisterType(typeof(UnitOfWorkManager)).InstancePerLifetimeScope();//工作单元注册为scope
            fsql.GlobalFilter.Apply<IDeleteAduitEntity>("IsDeleted", a => a.IsDeleted == false);

            try
            {
                using var objPool = fsql.Ado.MasterPool.Get();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e + e.StackTrace + e.Message + e.InnerException);
                return;
            }
            //在运行时直接生成表结构
            try
            {
                fsql.CodeFirst
                    .SeedData()
                    .SyncStructure(DomainReflexUtil.GetTypesByTableAttribute());
            }
            catch (Exception e)
            {
                Log.Logger.Error(e + e.StackTrace + e.Message + e.InnerException);
            }
        }


    }
}
