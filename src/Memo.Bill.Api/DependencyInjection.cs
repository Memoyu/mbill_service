using AspNetCoreRateLimit;
using Memo.Bill.Application.Common.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Memo.Bill.Api;

/// <summary>
/// Web APA 依赖注入
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 接口服务依赖注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        // 跨域配置
        services.AddCorsPolicy(configuration);

        // 限流
        services.AddRateLimit(configuration);

        services.AddControllers(options =>
           {
               // 禁用隐式的[Required]，为了统一响应模型
               options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
               // 处理数组接收带括号时不识别问题，主要用于get 数组传入
               options.ValueProviderFactories.Add(new JQueryQueryStringValueProviderFactory());
           }
        ).AddJsonOptions(opts => {
            opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            opts.JsonSerializerOptions.Converters.Insert(0, new AutoLongToStringConverter());
        });


        services.AddEndpointsApiExplorer();

        // Swagger 接口文档
        services.AddOpenApiDoc();

        return services;
    }

    private static IServiceCollection AddOpenApiDoc(this IServiceCollection services)
    {
        // 注册API文档
        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "Memo.Bill API";

            // Add JWT
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var policyOrigins = configuration.GetValue<string>("CorsOrigins") ?? string.Empty;
            options.AddPolicy(AppConst.CorsPolicyName, builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(policyOrigins.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray());
            });
        });

        return services;
    }

    private static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration configuration)
    {
        // 注入配置
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        // 存入到redis
        services.AddDistributedRateLimiting();

        return services;
    }
}
