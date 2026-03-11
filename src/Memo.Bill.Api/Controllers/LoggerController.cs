using Memo.Bill.Application.Loggers.Queries.System;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 日志管理
/// </summary>
public class LoggerController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 获取系统日志分页
    /// </summary>
    /// <returns></returns>
    [HttpGet("system/page")]
    public async Task<Result> PageSystemAsync([FromQuery] PageLoggerSystemQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取系统日志详情
    /// </summary>
    /// <returns></returns>
    [HttpGet("system/get")]
    public async Task<Result> GetSystemAsync([FromQuery] GetLoggerSystemQuery request)
    {
        return await mediator.Send(request);
    }
}
