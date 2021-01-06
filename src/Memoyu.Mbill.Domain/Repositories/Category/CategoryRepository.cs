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
