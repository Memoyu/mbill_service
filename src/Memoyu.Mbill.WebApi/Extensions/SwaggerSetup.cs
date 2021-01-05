/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：SwaggerSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:24:52
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Memoyu.Mbill.WebApi.Extensions
{
    /// <summary>
    /// 配置注册Swagger
    /// </summary>
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Memoyu.Web", Version = "v1" });
            });

            return services;
        }
    }
}
