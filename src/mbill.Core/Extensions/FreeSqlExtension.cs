using DataType = FreeSql.DataType;

namespace Mbill.Core.Extensions;

/// <summary>
/// FreeSql 扩展方法
/// </summary>
public static class FreeSqlExtension
{
    public static ISelect<TEntity> ToPage<TEntity>(this ISelect<TEntity> source, PagingDto pageDto, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pageDto.Page, pageDto.Size);
    }

    /// <summary>
    /// 分页处理，同步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageDto"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<TEntity> ToPageList<TEntity>(this ISelect<TEntity> source, PagingDto pageDto, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pageDto.Page, pageDto.Size).ToList();
    }

    /// <summary>
    /// 分页处理，异步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageDto"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Task<List<TEntity>> ToPageListAsync<TEntity>(this ISelect<TEntity> source, PagingDto pageDto, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pageDto.Page, pageDto.Size).ToListAsync();
    }

    /// <summary>
    /// 分页处理并映射Dto，同步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageDto"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<TResult> ToPageList<TEntity, TResult>(this ISelect<TEntity> source, PagingDto pageDto, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pageDto.Page, pageDto.Size).ToList<TResult>();
    }

    /// <summary>
    /// 分页处理并映射Dto，异步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageDto"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Task<List<TResult>> ToPageListAsync<TEntity, TResult>(this ISelect<TEntity> source, PagingDto pageDto, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pageDto.Page, pageDto.Size).ToListAsync<TResult>();
    }

    public static FreeSqlBuilder UseConnectionString(this FreeSqlBuilder builder)
    {
        var conStr = Appsettings.MySqlCon;
        if (!string.IsNullOrWhiteSpace(conStr))
        {
            builder.UseConnectionString(DataType.MySql, conStr);
        }
        else
        {
            Log.Error($"数据库配置ConnectionStrings:MySql无效");
        }

        return builder;
    }
    /// <summary>
    /// 请在UseConnectionString配置后调用此方法
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static FreeSqlBuilder CreateDatabaseIfNotExists(this FreeSqlBuilder builder)
    {
        FieldInfo dataTypeFieldInfo = builder.GetType().GetField("_dataType", BindingFlags.NonPublic | BindingFlags.Instance);

        if (dataTypeFieldInfo is null)
        {
            throw new ArgumentException("_dataType is null");
        }

        string connectionString = GetConnectionString(builder);
        DataType dbType = (DataType)dataTypeFieldInfo.GetValue(builder);

        switch (dbType)
        {
            case DataType.MySql:
                return builder.CreateDatabaseIfNotExistsMySql(connectionString);
            default:
                break;
        }

        Log.Error($"不支持创建数据库");
        return builder;
    }
    private static string GetConnectionString(FreeSqlBuilder builder)
    {
        Type type = builder.GetType();
        FieldInfo fieldInfo =
            type.GetField("_masterConnectionString", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo is null)
        {
            throw new ArgumentException("_masterConnectionString is null");
        }
        return fieldInfo.GetValue(builder).ToString();
    }

    public static FreeSqlBuilder CreateDatabaseIfNotExistsMySql(this FreeSqlBuilder builder,
       string connectionString = "")
    {
        if (connectionString == "")
        {
            connectionString = GetConnectionString(builder);
        }

        MySqlConnectionStringBuilder conStrBuilder = new MySqlConnectionStringBuilder(connectionString);

        string createDatabaseSql =
            $"USE mysql;CREATE DATABASE IF NOT EXISTS `{conStrBuilder.Database}` CHARACTER SET '{conStrBuilder.CharacterSet}' COLLATE 'utf8mb4_general_ci'";

        using MySqlConnection cnn = new MySqlConnection(
            $"Data Source={conStrBuilder.Server};Port={conStrBuilder.Port};User ID={conStrBuilder.UserID};Password={conStrBuilder.Password};Initial Catalog=mysql;Charset=utf8;SslMode=none;Max pool size=1");
        cnn.Open();
        using (MySqlCommand cmd = cnn.CreateCommand())
        {
            cmd.CommandText = createDatabaseSql;
            cmd.ExecuteNonQuery();
        }

        return builder;
    }

    private static string ExpandFileName(string fileName)
    {
        if (fileName.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
        {
            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (string.IsNullOrEmpty(dataDirectory))
            {
                dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            string name = fileName.Replace("\\", "").Replace("/", "").Substring("|DataDirectory|".Length);
            fileName = Path.Combine(dataDirectory, name);
        }
        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        }
        return Path.GetFullPath(fileName);
    }
}
