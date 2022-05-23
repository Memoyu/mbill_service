using FreeSql;
using mbill.Core.Interface.IRepositories.Core;
using mbill.Infrastructure.Repository.Base;
using mbill.Core.Security;
using mbill.Core.Domains.Entities.Core;

namespace mbill.Infrastructure.Repository.Core
{
    public class UserRoleRepo : AuditBaseRepo<UserRoleEntity>, IUserRoleRepo
    {
        private readonly ICurrentUser _currentUser;
        public UserRoleRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
