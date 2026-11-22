using Memo.Bill.Application.Notices.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 公告管理
    /// </summary>
    /// <param name="mediator"></param>
    public class NoticeController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> ListAsync([FromQuery] ListNoticeQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
