using Mbill.Core.Extensions.Converters;

namespace Mbill.Core.Extensions.ServiceCollection;

/// <summary>
/// 控制器配置注册
/// </summary>
public static class ControllerSetup
{
    public static IServiceCollection AddController(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add<InputFieldActionFilter>();
                options.Filters.Add<LocalExceptionFilter>();
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                opt.SerializerSettings.Converters.Add(new JsonLongToStringConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                    //自定义 BadRequest 响应
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState);

                    var resultDto = new ServiceResult
                    {
                        Code = ServiceResultCode.ParameterError,
                        Message = problemDetails.Errors
                    };

                    return new BadRequestObjectResult(resultDto)
                    {
                        ContentTypes = { "application/json" }
                    };
                };
                // 使用自定义模型验证【Api接口需要添加才能生效】
                options.SuppressModelStateInvalidFilter = true;
            });
        return services;
    }
}
