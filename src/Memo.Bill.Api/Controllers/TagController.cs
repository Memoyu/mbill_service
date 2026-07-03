using Memo.Bill.Application.Tags.Commands;
using Memo.Bill.Application.Tags.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 标签管理
    /// </summary>
    /// <param name="mediator"></param>
    public class TagController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<Result> CreateAsync(CreateTagCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<Result> UpdateAsync(UpdateTagCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新标签排序
        /// </summary>
        /// <returns></returns>
        [HttpPut("sort")]
        public async Task<Result> SortAsync(SortTagCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<Result> DeleteAsync([FromQuery] DeleteTagCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<Result> GetAsync([FromQuery] GetTagQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取标签分组
        /// </summary>
        /// <returns></returns>
        [HttpGet("list/group")]
        public async Task<Result> ListGroupAsync([FromQuery] ListGroupTagQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
