using Memo.Bill.Application.Bills.Commands;
using Memo.Bill.Application.Bills.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 账单管理
    /// </summary>
    /// <param name="mediator"></param>
    public class BillController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 创建账单
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<Result> CreateAsync(CreateBillCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新账单
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<Result> UpdateAsync(UpdateBillCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 删除账单
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<Result> DeleteAsync([FromQuery] DeleteBillCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取账单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<Result> GetAsync([FromQuery] GetBillQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取指定日期账单
        /// </summary>
        /// <returns></returns>
        [HttpGet("list/date")]
        public async Task<Result> GetBillDateAsync([FromQuery] GetBillDateQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 搜索账单
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<Result> SearchRecordAsync([FromQuery] SearchBillQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取搜索账单记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("search/record")]
        public async Task<Result> SearchBillRecordAsync([FromQuery] SearchBillRecordQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
