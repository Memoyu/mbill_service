using mbill_service.Core.Domains.Common.Options;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace mbill_service.Core.AOP.Middleware
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
                options.DocumentTitle = "mbill_service_ApiDoc";

            });
        }
    }
}
