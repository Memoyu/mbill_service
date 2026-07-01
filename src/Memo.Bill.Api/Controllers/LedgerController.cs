using Memo.Bill.Application.Ledgers.Commands;
using Memo.Bill.Application.Ledgers.Queries;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 账本管理
/// </summary>
/// <param name="mediator"></param>
public class LedgerController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 创建账本
    /// </summary>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<Result> CreateAsync(CreateLedgerCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 更新账本
    /// </summary>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<Result> UpdateAsync(UpdateLedgerCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 更新账本颜色
    /// </summary>
    /// <returns></returns>
    [HttpPut("update/color")]
    public async Task<Result> UpdateColorAsync(UpdateLedgerColorCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 更新账本排序
    /// </summary>
    /// <returns></returns>
    [HttpPut("sort")]
    public async Task<Result> SortAsync(SortLedgerCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 加入账本
    /// </summary>
    /// <returns></returns>
    [HttpPut("join")]
    public async Task<Result> JoinAsync(JoinLedgerUserCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取账本
    /// </summary>
    /// <returns></returns>
    [HttpGet("get")]
    public async Task<Result> GetAsync([FromQuery] GetLedgerQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 删除账本
    /// </summary>
    /// <returns></returns>
    [HttpDelete("delete")]
    public async Task<Result> DeleteAsync([FromQuery] DeleteLedgerCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取账本列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Result> ListAsync([FromQuery] ListLedgerQuery request)
    {
        return await mediator.Send(request);
    }
}
