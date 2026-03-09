using Memo.Bill.Application.Accounts.Commands;
using Memo.Bill.Application.Accounts.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    /// <param name="mediator"></param>
    public class AccountController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 创建账户
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<Result> CreateAsync(CreateAccountCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新账户
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<Result> UpdateAsync(UpdateAccountCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新账户排序
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/sort")]
        public async Task<Result> UpdateSortAsync(UpdateAccountSortCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<Result> DeleteAsync([FromQuery] DeleteAccountCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取账户
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<Result> GetAsync([FromQuery] GetAccountQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取账户分组
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/group")]
        public async Task<Result> GetGroupAsync([FromQuery] GetAccountGroupQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
