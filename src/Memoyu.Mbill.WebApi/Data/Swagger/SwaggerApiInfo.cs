/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Data.Swagger
*   文件名称 ：SwaggerApiInfo.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 18:59:49
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Memoyu.Mbill.Domain.Shared.Configurations;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using static Memoyu.Mbill.Domain.Shared.Const.SystemConst;

namespace Memoyu.Mbill.WebApi.Data.Swagger
{
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
        private static readonly string version = $"v{AppSettings.ApiVersion}";

        /// <summary>
        /// Swagger描述信息
        /// </summary>
        private static readonly string description = @"<b>Blog</b>：<a target="" _blank"" href=""http://memoyu.cn"">博客地址</a>&emsp;<b>GitHub</b>：<a target="" _blank"" href=""https://github.com/Memoyu"">Memoyu</a>&emsp;<b>Hangfire</b>：<a target="" _blank"" href=""https://localhost:44313/hangfire"">任务调度中心</a> <br /><br /> <code>Powered by .NET 5 on Linux</code>";

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
}
