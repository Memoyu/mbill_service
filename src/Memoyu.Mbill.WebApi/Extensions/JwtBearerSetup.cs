/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.WebApi.Extensions
*   文件名称 ：JwtBearerSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-11 22:00:53
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using DotNetCore.Security;
using Memoyu.Mbill.Domain.Shared.Configurations;
using Memoyu.Mbill.ToolKits.Base;
using Memoyu.Mbill.ToolKits.Base.Enum.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Memoyu.Mbill.WebApi.Extensions
{
    /// <summary>
    /// 配置Token
    /// </summary>
    public static class JwtBearerSetup
    {
        public static void AddJwtBearer(this IServiceCollection services)
        {
            var jsonWebTokenSetting = services.AddSecurity();
            services.AddAuthentication(opts =>//认证方式
            {
                opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>//Cookie
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>//配置JWT
            {
                bool isIds4 = AppSettings.IdentityServer4Enable;

                if (isIds4)
                {
                    //identityserver4 地址
                    options.Authority = AppSettings.Authority;
                }
                options.RequireHttpsMetadata = AppSettings.IsUseHttps;
                options.Audience = AppSettings.ClientName;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 密钥必须匹配
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jsonWebTokenSetting.SecurityKey,

                    // 验证Issuer
                    ValidateIssuer = true,
                    ValidIssuer = jsonWebTokenSetting.Issuer,

                    // 验证Audience
                    ValidateAudience = true,
                    ValidAudience = jsonWebTokenSetting.Audience,

                    // 验证过期时间
                    ValidateLifetime = true,

                    //偏移设置为了0s,用于测试过期策略,完全按照access_token的过期时间策略，默认原本为5分钟
                    ClockSkew = TimeSpan.Zero
                };


                //使用Authorize设置为需要登录时，返回json格式数据。
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token 过期
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                        context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>//此处为权限验证失败后触发的事件
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦
                        context.HandleResponse();

                        string message;
                        ServiceResultCode code;
                        int statusCode = StatusCodes.Status401Unauthorized;

                        if (context.Error == "invalid_token" && context.ErrorDescription == "The token is expired")//Token过期
                        {
                            message = "令牌过期";
                            code = ServiceResultCode.TokenExpired;
                            statusCode = StatusCodes.Status422UnprocessableEntity;
                        }
                        else if (context.Error == "invalid_token" && context.ErrorDescription.IsNullOrEmpty())//Token失效
                        {
                            message = "令牌失效";
                            code = ServiceResultCode.TokenInvalidation;
                        }
                        else
                        {
                            message = "请先登录 " + context.ErrorDescription; 
                            code = ServiceResultCode.AuthenticationFailed;
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = statusCode;
                        await context.Response.WriteAsync(new ServiceResult(code, message).ToString());

                    }
                };
            });
        }

        private static JsonWebTokenSettings AddSecurity(this IServiceCollection services)
        {
            JsonWebTokenSettings jsonWebTokenSettings = new JsonWebTokenSettings(
                           AppSettings.JwtBearer.SecurityKey,
                           TimeSpan.FromMinutes(AppSettings.JwtBearer.Expires),
                           AppSettings.JwtBearer.Audience,
                           AppSettings.JwtBearer.Issuer
                       );
            services.AddHash();
            services.AddCryptography("Memoyu-cryptography");
            services.AddJsonWebToken(jsonWebTokenSettings);
            return jsonWebTokenSettings;
        }
    }
}
