using Memo.Bill.Application.Permissions.Queries;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 权限管理
/// </summary>
public class PermissionController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 权限列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Result> ListAsync([FromQuery] ListPermissionQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 权限分组列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("group")]
    public async Task<Result> GroupAsync([FromQuery] GroupPermissionQuery request)
    {
        return await mediator.Send(request);
    }
}
