using Memo.Bill.Application.Users.Commands;
using Memo.Bill.Application.Users.Queries;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 用户管理
/// </summary>
public class UserController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<Result> CreateAsync(CreateUserCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<Result> UpdateAsync(UpdateUserCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 变更用户密码
    /// </summary>
    /// <returns></returns>
    [HttpPut("change-password")]
    public async Task<Result> ChangePasswordAsync(ChangePasswordCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <returns></returns>
    [HttpDelete("delete")]
    public async Task<Result> DeleteAsync([FromQuery] DeleteUserCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <returns></returns>
    [HttpGet("get")]
    public async Task<Result> GetAsync([FromQuery] GetUserQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<Result> PageAsync([FromQuery] PageUserQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// 获取用户选项列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("list/select")]
    public async Task<Result> ListSelectAsync([FromQuery] SelectUserQuery request)
    {
        return await mediator.Send(request);
    }
}
