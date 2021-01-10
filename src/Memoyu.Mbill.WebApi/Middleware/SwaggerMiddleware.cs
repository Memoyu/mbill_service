/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Middleware
*   文件名称 ：SwaggerMiddleware.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-09 18:53:33
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.WebApi.Data.Swagger;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Memoyu.Mbill.WebApi.Middleware
{
    public static class SwaggerMiddleware
    {
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                ApiInfo.ApiInfos.ForEach(a => options.SwaggerEndpoint($"/swagger/{a.UrlPrefix}/swagger.json", a.Name));

                //模型默认扩展深度，-1位完全隐藏
                options.DefaultModelExpandDepth(-1);
                //API仅展开标记
                options.DocExpansion(DocExpansion.List);
                //API前缀设为空
                options.RoutePrefix = string.Empty;
                //API页面标题
                options.DocumentTitle = "Mbill_ApiDoc";

            });
        }
    }
}
