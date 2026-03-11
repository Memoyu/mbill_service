using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Memo.Bill.Application.Common.Services.Background;

/// <summary>
/// 定时任务演示
/// </summary>
/// <param name="serviceScopeFactory"></param>
/// <param name="logger"></param>
internal class DemoTaskService(
     IServiceScopeFactory serviceScopeFactory,
     ILogger<DemoTaskService> logger
     ) : BaseTaskService(logger)
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await ExecuteScheduledTaskAsync(
          cancellationToken,
          ScheduledTaskTimeType.Time,
          new TimeSpan(1, 0, 0), // 每小时执行一次
          async () =>
          {
              await ExecuteJobAsync(cancellationToken);
          });
    }

    private async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // 解决BackgroundService is Singleton Service
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        // 触发后台任务事件
        // await publisher.Publish(new SomeEvent(), cancellationToken);
        await Task.CompletedTask;
    }

}
