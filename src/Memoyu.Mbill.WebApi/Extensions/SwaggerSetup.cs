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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
                //opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Memoyu.MBill.Domain.xml"));
                //opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Memoyu.MBill.Application.Contracts.xml"));

                #region 小绿锁

                #region Bearer
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Id =  "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization", //jwt默认的参数名称
                    In = ParameterLocation.Header, //jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion

                #region Oauth2
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Id =  "oauth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "LinCms.Web", "Access read/write LinCms.Web" }
                            }
                        },
                        Password = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Access read openid" },
                                { "offline_access", "Access read offline_access" },
                                { "LinCms.Web", "Access read/write LinCms.Web" }
                            }
                        }
                    }
                }); 
                #endregion

                #endregion

                //opt.OperationFilter<AddResponseHeadersFilter>();
                //opt.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //opt.OperationFilter<SecurityRequirementsOperationFilter>();

                // 应用Controller的API文档描述信息
                opt.DocumentFilter<SwaggerDocumentFilter>();
            });




            return services;
        }

    }
}
