namespace mbill.Core.Domains.Common.Options;

public class SwaggerApiInfo
{
    /// <summary>
    /// URL前缀
    /// </summary>
    public string UrlPrefix { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 可配置参数
    /// </summary>
    public OpenApiInfo OpenApiInfo { get; set; }
}

public class ApiInfo
{

    /// <summary>
    /// 当前API版本，从appsettings.json获取
    /// </summary>
    private static readonly string version = $"v{Appsettings.ApiVersion}";

    /// <summary>
    /// Swagger描述信息
    /// </summary>
    private static readonly string description = @"<b>Blog</b>：<a target="" _blank"" href=""http://memoyu.cn"">博客地址</a>&emsp;<b>GitHub</b>：<a target="" _blank"" href=""https://github.com/Memoyu"">Memoyu</a>&emsp;<b>Hangfire</b>：<a target="" _blank"" href=""https://localhost:44313/hangfire"">任务调度中心</a> <br /><br /> <code>Powered by .NET 6 on Linux</code>";

    /// <summary>
    /// Swagger分组信息
    /// </summary>
    public static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>
        {
            new SwaggerApiInfo
            {
                Name = "前台客户端接口",
                UrlPrefix = Grouping.GroupName_v1,
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "mbill - 前台客户端接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                Name = "后台管理端接口",
                UrlPrefix = Grouping.GroupName_v2,
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "mbill - 后台管理端接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                Name = "通用公共接口",
                UrlPrefix = Grouping.GroupName_v3,
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "mbill - 通用公共接口",
                    Description = description
                }
            }
        };
}
