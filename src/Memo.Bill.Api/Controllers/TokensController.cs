using Memo.Bill.Application.Tokens.Commands.Create;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 用户授权
/// </summary>
/// <param name="mediator"></param>
[AllowAnonymous]
public class TokensController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 生成用户Token
    /// </summary>
    /// <param name="request">用户账户、密码</param>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<Result> CreateAsync(CreateTokenQuery request)
    {
        return await mediator.Send(request);
    }
}
