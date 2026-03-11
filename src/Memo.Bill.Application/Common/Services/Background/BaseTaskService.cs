using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Memo.Bill.Application.Common.Services.Background;

internal abstract class BaseTaskService : BackgroundService
{
    private readonly ILogger _logger;

    public BaseTaskService(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 运行周期的任务
    /// </summary>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <param name="type">时间类型 0：小时、分钟、秒；1：天；</param>
    /// <param name="time">TimeSpan</param>
    /// <param name="nextTickHandle">周期任务</param>
    /// <returns>returns</returns>
    public async Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken, ScheduledTaskTimeType type, TimeSpan time, Func<Task> nextTickHandle)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var delay = CalcDelayToNextTime(type, time);
            _logger.LogInformation($"{GetType().Name}自动化程序，将在{delay}后处理");
            await Task.Delay(delay, cancellationToken);
            await nextTickHandle();
        }
    }

    private TimeSpan CalcDelayToNextTime(ScheduledTaskTimeType type, TimeSpan momentTime)
    {
        switch (type)
        {
            case ScheduledTaskTimeType.Time:
                return DateTime.Now.Add(momentTime) - DateTime.Now;
            case ScheduledTaskTimeType.Day:
                return DateTime.Now.TimeOfDay < momentTime ?
                        DateTime.Now.Date.Add(momentTime) - DateTime.Now : // 延迟到今天
                        DateTime.Now.Date.AddDays(1).Add(momentTime) - DateTime.Now; // 延迟到明天
            default:
                throw new Exception("未指定的时间类型");
        }
    }
}

internal enum ScheduledTaskTimeType
{
    Time, // 小时、分钟、秒
    Day, // 天
}
