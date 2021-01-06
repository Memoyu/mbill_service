/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Repositories.Category
*   文件名称 ：CategoryRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Entities.Category;
using Memoyu.Mbill.Domain.IRepositories.Category;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.Category
{
    public class CategoryRepository : AuditBaseRepository<CategoryEntity>, ICategoryRepository
    {
        private readonly ICurrentUser _currentUser;
        public CategoryRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
