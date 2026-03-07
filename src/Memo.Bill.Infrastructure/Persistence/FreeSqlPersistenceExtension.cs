using System.Configuration;
using System.Reflection;
using FreeSql;
using FreeSql.DataAnnotations;
using FreeSql.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using MySql.Data.MySqlClient;
using Serilog;

namespace Memo.Bill.Infrastructure.Persistence;

public static class FreeSqlPersistenceExtension
{
    /// <summary>
    /// 注册MySql数据持久化组件
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPersistenceForMyql(this IServiceCollection services, IConfiguration configuration)
    {
        IFreeSql fsql = new FreeSqlBuilder()
          .UseMysqlConnectionString(configuration, true)
          .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
          .UseAutoSyncStructure(true)
          .UseNoneCommandParameter(true)
          .UseMonitorCommand(cmd =>
          {
              // Trace.WriteLine(cmd.CommandText + ";");
          })
          .Build()
          .SetDbContextOptions(opt =>
          {
              opt.EnableCascadeSave = true;
          });//联级保存功能开启（默认为关闭）

        fsql.Aop.CurdAfter += (s, e) =>
        {
            if (e.ElapsedMilliseconds > 200)
            {
                //记录日志
                //发送短信给负责人
            }
        };

        // 属性配置
        fsql.Aop.ConfigEntityProperty += (s, e) =>
        {
            if (e.Property.PropertyType.IsEnum)
                e.ModifyResult.MapType = typeof(int);
        };

        fsql.Aop.AuditValue += (s, e) =>
        {
            if (e.Column.CsType == typeof(long) && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null && e.Value?.ToString() == "0")
                e.Value = SnowFlakeUtil.NextId();
        };

        fsql.GlobalFilter.Apply<BaseAuditEntity>("IsDeleted", a => a.IsDeleted == false); // 全局过滤字段

        // 注册FreeSql and UnitOfWorkManager
        services.AddSingleton(fsql);
        services.TryAddScoped<UnitOfWorkManager>();

        // 批量注册复合主键的 Repository
        services.TryAddScoped(typeof(IBaseDefaultRepository<>), typeof(BaseDefaultRepository<>));

        //在运行时直接生成表结构
        try
        {
            fsql.CodeFirst.SyncStructure(GetTypesByTableAttribute());
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "生成表结构异常" + ex.Message);
        }

        return services;
    }

    public static FreeSqlBuilder UseMysqlConnectionString(this FreeSqlBuilder builder, IConfiguration configuration, bool createDatabaseIfNotExists = false)
    {
        var connectionString = configuration.GetConnectionString("MySql") ?? string.Empty;
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, "数据库连接不能为空");

        builder.UseConnectionString(DataType.MySql, connectionString);

        if (createDatabaseIfNotExists)
            CreateDatabaseIfNotExistsMySql(connectionString);

        return builder;
    }

    /// <summary>
    /// 注册MongoDb持久化组件
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPersistenceForMongo(this IServiceCollection services, IConfiguration configuration)
    {
        // 注册MongoDb配置
        var section = configuration.GetSection(AppConst.MongoSection) ?? throw new ConfigurationErrorsException(AppConst.MongoSection + " section is not null");
        services.Configure<MongoOptions>(section);

        var mongoOptions = section.Get<MongoOptions>() ?? throw new ConfigurationErrorsException(AppConst.MongoSection + " is not null");

        services.AddSingleton(new MongoClient(mongoOptions.ConnectionString));

        // 批量注册复合主键的 Repository
        services.TryAddScoped(typeof(IBaseMongoRepository<>), typeof(BaseMongoRepository<>));

        return services;
    }

    private static void CreateDatabaseIfNotExistsMySql(string connectionString)
    {
        MySqlConnectionStringBuilder conStrBuilder = new MySqlConnectionStringBuilder(connectionString);
        string createDatabaseSql =
            $"USE mysql;CREATE DATABASE IF NOT EXISTS `{conStrBuilder.Database}` CHARACTER SET '{conStrBuilder.CharacterSet}' COLLATE 'utf8mb4_general_ci'";

        using MySqlConnection cnn = new MySqlConnection(
            $"Data Source={conStrBuilder.Server};Port={conStrBuilder.Port};User ID={conStrBuilder.UserID};Password={conStrBuilder.Password};Initial Catalog=mysql;Charset=utf8;SslMode=none;Max pool size=1");
        cnn.Open();
        using MySqlCommand cmd = cnn.CreateCommand();
        cmd.CommandText = createDatabaseSql;
        cmd.ExecuteNonQuery();
    }

    private static Type[] GetTypesByTableAttribute()
    {
        List<Type> tableAssembies = [];
        var types = Assembly.GetAssembly(typeof(BaseAuditEntity))?.GetExportedTypes() ?? [];
        foreach (Type type in types)
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                if (attribute is TableAttribute tableAttribute)
                {
                    if (tableAttribute.DisableSyncStructure == false)
                    {
                        tableAssembies.Add(type);
                    }
                }
            }
        };
        return [.. tableAssembies];
    }
}
