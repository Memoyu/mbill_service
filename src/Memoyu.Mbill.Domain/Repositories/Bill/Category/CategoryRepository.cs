/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Repositories.Bill.Category
*   文件名称 ：CategoryRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;
using Memoyu.Mbill.Domain.Entities.Bill.Category;
using Memoyu.Mbill.Domain.IRepositories.Bill.Category;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Repositories.Bill.Category
{
    public class CategoryRepository : AuditBaseRepository<CategoryEntity>, ICategoryRepository
    {
        private readonly ICurrentUser _currentUser;
        public CategoryRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task<CategoryEntity> GetCategoryAsync(long id)
        {
            return await GetAsync(id);
        }

        public async Task<CategoryEntity> GetCategoryParentAsync(long id)
        {
            var asset = await GetAsync(id);
            if (asset == null) return null;
            return await GetAsync(asset.ParentId);
        }
    }
}
