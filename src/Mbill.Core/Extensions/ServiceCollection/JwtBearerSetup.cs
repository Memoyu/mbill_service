namespace Mbill.Core.Extensions.ServiceCollection;

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
                OnChallenge = async context =>
                {
                    //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦
                    context.HandleResponse();

                    string message;
                    ServiceResultCode code;
                    int statusCode = StatusCodes.Status401Unauthorized;

                    if (context.Error == "invalid_token" && context.ErrorDescription.StartsWith("The token expired at"))//Token过期
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
                        message = "请先登录 ";
                        code = ServiceResultCode.AuthenticationFailed;
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsync(new ServiceResult(code, message).ToString());

                }
            };
        });
    }

    private static JwtSettings AddSecurity(this IServiceCollection services)
    {
        JwtSettings jsonWebTokenSettings = new JwtSettings(
                       Appsettings.JwtBearer.SecurityKey,
                       TimeSpan.FromSeconds(Appsettings.JwtBearer.Expires),
                       Appsettings.JwtBearer.Audience,
                       Appsettings.JwtBearer.Issuer
                   );
        services.AddHashService();
        services.AddICryptographyService("Memoyu.Core-cryptography");
        services.AddJwtService(jsonWebTokenSettings);
        return jsonWebTokenSettings;
    }
}
