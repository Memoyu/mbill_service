using mbill_service.Core.Domains.Common.Options;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace mbill_service.Core.AOP.Middleware
{
    public static class SwaggerMiddleware
    {
        public static void UseSwaggerUI(this IApplicationBuilder app , Func<Stream> streamHtml)
        {
            app.UseSwaggerUI(options =>
            {
                
                ApiInfo.ApiInfos.ForEach(a => options.SwaggerEndpoint($"/swagger/{a.UrlPrefix}/swagger.json", a.Name));

                // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：{项目名.index.html}
                if (streamHtml.Invoke() == null)
                {
                    var msg = "index.html的属性，必须设置为嵌入的资源";
                    throw new Exception(msg);
                }
                options.IndexStream = streamHtml;
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
