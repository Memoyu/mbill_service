using Memo.Bill.Application.Categories.Commands;
using Memo.Bill.Application.Categories.Queries;

namespace Memo.Bill.Api.Controllers
{
    /// <summary>
    /// 分类管理
    /// </summary>
    /// <param name="mediator"></param>
    public class CategoryController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 创建分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<Result> CreateAsync(CreateCategoryCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<Result> UpdateAsync(UpdateCategoryCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 更新分类排序
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/sort")]
        public async Task<Result> UpdateSortAsync(UpdateCategorySortCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<Result> DeleteAsync([FromQuery] DeleteCategoryCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取账户
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<Result> GetAsync([FromQuery] GetCategoryQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取分类分组
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/group")]
        public async Task<Result> GetGroupAsync([FromQuery] GetCategoryGroupQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
