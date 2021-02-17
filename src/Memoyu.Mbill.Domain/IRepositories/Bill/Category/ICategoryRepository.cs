/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.IRepositories.Bill.Category
*   文件名称 ：ICategoryRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.IRepositories.Bill.Category
{
    public interface ICategoryRepository : IAuditBaseRepository<CategoryEntity>
    {
        /// <summary>
        /// 获取分类信息 By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CategoryEntity> GetCategoryAsync(long id);

        /// <summary>
        /// 获取分类父项 By 子项 Id
        /// </summary>
        /// <param name="id">分类子项Id</param>
        /// <returns></returns>
        Task<CategoryEntity> GetCategoryParentAsync(long id);

    }
}
