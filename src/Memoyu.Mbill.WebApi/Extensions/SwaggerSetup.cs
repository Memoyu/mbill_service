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
using Memoyu.Mbill.WebApi.Aop.Filter;
using Memoyu.Mbill.WebApi.Data.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;

namespace Memoyu.Mbill.WebApi.Extensions
{
    /// <summary>
    /// 配置注册Swagger
    /// </summary>
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                //遍历应用Swagger分组信息
                ApiInfo.ApiInfos.ForEach(a => opt.SwaggerDoc(a.UrlPrefix, a.OpenApiInfo));

                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Memoyu.MBill.WebApi.xml"));
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Memoyu.MBill.Domain.xml"));
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Memoyu.MBill.Application.Contracts.xml"));

                #region 小绿锁

                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入 Bearer {Token} 进行身份验证",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                opt.AddSecurityDefinition("oauth2", security);
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                opt.OperationFilter<AddResponseHeadersFilter>();
                opt.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                opt.OperationFilter<SecurityRequirementsOperationFilter>();

                #endregion

                // 应用Controller的API文档描述信息
                opt.DocumentFilter<SwaggerDocumentFilter>();
            });




            return services;
        }

    }
}
