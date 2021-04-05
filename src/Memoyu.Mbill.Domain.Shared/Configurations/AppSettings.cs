/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Shared.Configurations
*   文件名称 ：AppSettings.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 10:42:43
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Memoyu.Mbill.Domain.Shared.Configurations
{

    public class AppSettings
    {
        private static readonly IConfigurationRoot _configuration;
        static AppSettings()
        {
            _configuration = new ConfigurationBuilder()//配置配置文件
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Config/SerilogConfig.json", optional: true, reloadOnChange: true)//添加Serilog配置
                .AddJsonFile($"Config/RateLimitConfig.json", optional: true, reloadOnChange: true)//添加限流配置
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
   
        }
        #region System

        public static IConfiguration Configuration => _configuration;

        /// <summary>
        /// 接口版本
        /// </summary>
        public static string ApiVersion => _configuration["ApiVersion"];

        #endregion

        #region Authentication

        /// <summary>
        /// 是否开启IdentityServer4
        /// </summary>
        public static bool IdentityServer4Enable => Convert.ToBoolean(_configuration["Service:UseIdentityServer4"] ?? "false");

        /// <summary>
        /// Ids4 服务地址
        /// </summary>
        public static string Authority => _configuration["Service:Authority"];

        /// <summary>
        /// 是否使用Https
        /// </summary>
        public static bool IsUseHttps => Convert.ToBoolean(_configuration["Service:UseHttps"]);

        /// <summary>
        /// ClientName
        /// </summary>
        public static string ClientName => _configuration["Service:ClientName"];


        /// <summary>
        /// Jwt Token Config
        /// </summary>
        public class JwtBearer
        {
            /// <summary>
            /// 密钥
            /// </summary>
            public static string SecurityKey => _configuration["Authentication:JwtBearer:SecurityKey"];

            /// <summary>
            /// Audience
            /// </summary>
            public static string Audience => _configuration["Authentication:JwtBearer:Audience"];

            /// <summary>
            /// 过期时间(分钟)
            /// </summary>
            public static double Expires => Convert.ToDouble(_configuration["Authentication:JwtBearer:Expires"]);

            /// <summary>
            /// 签发者
            /// </summary>
            public static string Issuer =>_configuration["Authentication:JwtBearer:Issuer"];
        }


        #endregion

        #region Cors

        public static class Cors 
        {
            /// <summary>
            /// 跨域策略名
            /// </summary>
            public static string CorsName => _configuration["Cors:Name"];
            /// <summary>
            /// 跨域源
            /// </summary>
            public static string CorsOrigins => _configuration["Cors:Origins"];
        }

        #endregion

        #region Db

        /// <summary>
        /// 获取配置默认Db Code
        /// </summary>
        public static string DbTypeCode => _configuration["ConnectionStrings:DefaultDB"];

        /// <summary>
        /// 获取配置 MySql ConnectionString
        /// </summary>
        public static string MySqlCon => _configuration["ConnectionStrings:MySql"];

        /// <summary>
        /// 获取配置默认Db ConnectionString 
        /// </summary>
        public static string DbConnectionString(string dbTypeCode) => _configuration[$"ConnectionStrings:{dbTypeCode}"];

        #endregion

        #region Cache

        /// <summary>
        /// 是否开启Cache
        /// </summary>
        public static bool CacheEnable => Convert.ToBoolean(_configuration["Cache:Enable"]);

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public static int CacheExpire => Convert.ToInt32(_configuration["Cache:ExpireSeconds"]);

        #endregion

        #region IP

        /// <summary>
        /// 是否开启IP记录
        /// </summary>
        public static bool IpLogEnable => Convert.ToBoolean(_configuration["Middleware:IPLog:Enabled"]);

        /// <summary>
        /// 是否开启IP限流
        /// </summary>
        public static bool IpRateLimitEnable => Convert.ToBoolean(_configuration["Middleware:IpRateLimit:Enabled"]);
        public static IConfigurationSection IpRateLimitingConfig => _configuration.GetSection("IpRateLimiting");
        public static IConfigurationSection IpRateLimitPoliciesConfig => _configuration.GetSection("IpRateLimitPolicies");

        #endregion

        #region CAP

        /// <summary>
        /// Cap默认存储
        /// </summary>
        public static string CapDefaultStorage => _configuration["CAP:DefaultStorage"];

        /// <summary>
        /// Cap默认队列
        /// </summary>
        public static string CapDefaultMessageQueue => _configuration["CAP:DefaultMessageQueue"];

        /// <summary>
        /// Cap RabbitMq 连接信息
        /// </summary>
        public class CapRabbitMq
        {
            public static string HostName => _configuration["CAP:RabbitMQ:HostName"];
            public static string UserName => _configuration["CAP:RabbitMQ:UserName"];
            public static string Password => _configuration["CAP:RabbitMQ:Password"];
            public static string VirtualHost => _configuration["CAP:RabbitMQ:VirtualHost"];
        }

        #endregion

        #region Redis

        /// <summary>
        /// CsRedis连接字符串
        /// </summary>
        public static string CsRedisCon => _configuration["ConnectionStrings:CsRedis"];

        #endregion

    }
}
