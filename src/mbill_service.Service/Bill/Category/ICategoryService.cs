using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Service.Bill.Category.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mbill_service.Service.Bill.Category
{
    public interface ICategoryService
    {
        /// <summary>
        /// 新增账单分类
        /// </summary>
        /// <param name="categroy">数据源</param>
        /// <returns></returns>
        Task InsertAsync( CategoryEntity categroy);

        /// <summary>
        /// 更新账单分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categroy"></param>
        /// <returns></returns>
        Task UpdateAsync(long id, CategoryEntity categroy);

        Task DeleteAsync(long id);

        /// <summary>
        /// 获取分级后的组合类别数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<CategoryGroupDto>> GetGroupsAsync(string type);

        Task<IEnumerable<CategoryDto>> GetListAsync();

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CategoryDto> GetAsync(long id);

        /// <summary>
        /// 获取父项 by 子项 id
        /// </summary>
        /// <param name="id">分类子项Id</param>
        /// <returns></returns>
        Task<CategoryDto> GetParentAsync(long id);

    }
}
