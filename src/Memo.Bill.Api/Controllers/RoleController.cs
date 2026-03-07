using Memo.Bill.Application.Roles.Commands.Create;
using Memo.Bill.Application.Roles.Commands.Delete;
using Memo.Bill.Application.Roles.Commands.Update;
using Memo.Bill.Application.Roles.Queries.Get;
using Memo.Bill.Application.Roles.Queries.List;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 角色管理
/// </summary>
public class RoleController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<Result> CreateAsync(CreateRoleCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<Result> UpdateAsync(UpdateRoleCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <returns></returns>
    [HttpDelete("delete")]
    public async Task<Result> DeleteAsync([FromQuery] DeleteRoleCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <returns></returns>
    [HttpGet("get")]
    public async Task<Result> GetAsync([FromQuery] GetRoleQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 角色列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Result> ListAsync([FromQuery] ListRoleQuery request)
    {
        return await mediator.Send(request);
    }

}
