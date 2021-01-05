/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Shared.Extensionns
*   文件名称 ：FreeSqlExtension.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 10:35:30
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Serilog;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace Memoyu.Mbill.Domain.Shared.Extensions
{
    /// <summary>
    /// FreeSql 扩展方法
    /// </summary>
    public static class FreeSqlExtension
    {
        public static FreeSqlBuilder UseConnectionString(this FreeSqlBuilder builder, IConfiguration configuration)
        {
            IConfigurationSection dbTypeCode = configuration.GetSection("ConnectionStrings:DefaultDB");
            if (Enum.TryParse(dbTypeCode.Value, out DataType dataType))
            {
                if (!Enum.IsDefined(typeof(DataType), dataType))
                {
                    Log.Error($"数据库配置ConnectionStrings:DefaultDB:{dataType}无效");
                }

                IConfigurationSection configurationSection = configuration.GetSection($"ConnectionStrings:{dataType}");
                builder.UseConnectionString(dataType, configurationSection.Value);
            }
            else
            {
                Log.Error($"数据库配置ConnectionStrings:DefaultDB:{dbTypeCode.Value}无效");
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
                case DataType.SqlServer:
                    return builder.CreateDatabaseIfNotExistsSqlServer(connectionString);
                case DataType.PostgreSQL:
                    break;
                case DataType.Oracle:
                    break;
                case DataType.Sqlite:
                    return builder;
                case DataType.OdbcOracle:
                    break;
                case DataType.OdbcSqlServer:
                    break;
                case DataType.OdbcMySql:
                    break;
                case DataType.OdbcPostgreSQL:
                    break;
                case DataType.Odbc:
                    break;
                case DataType.OdbcDameng:
                    break;
                case DataType.MsAccess:
                    break;
                case DataType.Dameng:
                    break;
                case DataType.OdbcKingbaseES:
                    break;
                case DataType.ShenTong:
                    break;
                case DataType.KingbaseES:
                    break;
                case DataType.Firebird:
                    break;
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

        public static FreeSqlBuilder CreateDatabaseIfNotExistsSqlServer(this FreeSqlBuilder builder, string connectionString = "")
        {
            if (connectionString == "")
            {
                connectionString = GetConnectionString(builder);
            }
            SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder(connectionString);
            string createDatabaseSql;
            if (!string.IsNullOrEmpty(conStrBuilder.AttachDBFilename))
            {
                string fileName = ExpandFileName(conStrBuilder.AttachDBFilename);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string logFileName = Path.ChangeExtension(fileName, ".ldf");
                createDatabaseSql = @$"CREATE DATABASE {conStrBuilder.InitialCatalog}   on  primary   
                (
                    name = '{name}',
                    filename = '{fileName}'
                )
                log on
                (
                    name= '{name}_log',
                    filename = '{logFileName}'
                )";
            }
            else
            {
                createDatabaseSql = @$"CREATE DATABASE {conStrBuilder.InitialCatalog}";
            }

            using SqlConnection cnn =
                new SqlConnection(
                    $"Data Source={conStrBuilder.DataSource};Integrated Security = True;User ID={conStrBuilder.UserID};Password={conStrBuilder.Password};Initial Catalog=master;Min pool size=1");
            cnn.Open();
            using SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = $"select * from sysdatabases where name = '{conStrBuilder.InitialCatalog}'";

            SqlDataAdapter apter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            apter.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
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
}
