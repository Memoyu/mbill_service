using AspNetCoreRateLimit;
using Memo.Bill.Domain.Events.Permissions;

namespace Memo.Bill.Api;

/// <summary>
/// 中间件扩展
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// 添加管道中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseAppMiddleware(this WebApplication app)
    {
        InitializeAppData(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        // 限流
        app.UseIpRateLimiting();

        app.UseCors(AppConst.CorsPolicyName);

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    /// <summary>
    /// 初始化应用数据（如权限数据同步）
    /// </summary>
    /// <param name="app"></param>
    private static async Task InitializeAppData(WebApplication app)
    {
        // 依赖容器构建完成，做数权限数据同步
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var publisher = services.GetRequiredService<IPublisher>();
        await publisher.Publish(new SyncPermissionEvent());
    }
}
