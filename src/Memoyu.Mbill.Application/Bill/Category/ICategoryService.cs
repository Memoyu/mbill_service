/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Bill.Category
*   文件名称 ：ICategoryService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:14:50
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Application.Contracts.Dtos.Bill.Category;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Application.Bill.Category
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

        Task<CategoryDto> GetAsync(long id);

    }
}
