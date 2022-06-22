using Module = Autofac.Module;

namespace mbill.Modules;

public class FreeSqlModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        IFreeSql fsql = new FreeSqlBuilder()
          .UseConnectionString(Appsettings.Configuration)
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
        fsql.GlobalFilter.Apply<IDeleteAduitEntity>("IsDeleted", a => a.IsDeleted == false); // 全局过滤字段

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
                .SyncStructure(DomainReflexUtil.GetTypesByTableAttribute());
        }
        catch (Exception e)
        {
            Log.Logger.Error(e + e.StackTrace + e.Message + e.InnerException);
        }
    }
}
