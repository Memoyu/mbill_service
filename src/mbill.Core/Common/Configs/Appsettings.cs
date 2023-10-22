namespace Mbill.Core.Common.Configs;

public class Appsettings
{
    private static readonly IConfigurationRoot _configuration;
    static Appsettings()
    {
        _configuration = new ConfigurationBuilder()//配置配置文件
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"Configs/RateLimitConfig.json", optional: true, reloadOnChange: true)//添加限流配置
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

    #region Authentication

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
        /// 过期时间(秒)
        /// </summary>
        public static double Expires => Convert.ToDouble(_configuration["Authentication:JwtBearer:Expires"]);

        /// <summary>
        /// 签发者
        /// </summary>
        public static string Issuer => _configuration["Authentication:JwtBearer:Issuer"];
    }

    #endregion

    #region MinPro

    /// <summary>
    /// MinPro
    /// </summary>
    public class MinPro
    {
        /// <summary>
        /// AppID
        /// </summary>
        public static string AppID => _configuration["MinPro:AppID"];

        /// <summary>
        /// AppSecret
        /// </summary>
        public static string AppSecret => _configuration["MinPro:AppSecret"];

    }


    #endregion

    #region Db

    /// <summary>
    /// 获取配置 MySql ConnectionString
    /// </summary>
    public static string MySqlCon => _configuration["ConnectionStrings:MySql"];


    /// <summary>
    /// MongoDB连接字符串
    /// </summary>
    public static string MongoDBCon => _configuration["ConnectionStrings:MongoDB:ConnStr"];

    /// <summary>
    /// MongoDB DatabaseName
    /// </summary>
    public static string MongoDBName => _configuration["ConnectionStrings:MongoDB:DatabaseName"];

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
    /// Cap默认存储表前缀
    /// </summary>
    public static string CapStorageTablePrefix => _configuration["CAP:TableNamePrefix"];

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
    /// Redis连接字符串
    /// </summary>
    public static string RedisCon => _configuration["ConnectionStrings:Redis"];

    #endregion
}
