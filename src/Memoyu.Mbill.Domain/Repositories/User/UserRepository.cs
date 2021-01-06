using FreeSql;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.IRepositories.User;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.User
{
    public class UserRepository : AuditBaseRepository<UserEntity>, IUserRepository
    {
        private readonly ICurrentUser _currentUser;
        public UserRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
