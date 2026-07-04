using Memo.Bill.Application.Icons.Command;
using Memo.Bill.Application.Icons.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 图标资源
    /// </summary>
    /// <param name="mediator"></param>
    public class IconController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 同步图标资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("sync")]
        public async Task<Result> SyncAsync([FromBody] SyncIconCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取图标目录
        /// </summary>
        /// <returns></returns>
        [HttpGet("list/catalog")]
        public async Task<Result> ListCatalogAsync([FromQuery] ListCatalogIconQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
