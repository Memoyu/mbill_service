using FreeSql;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Infrastructure.Repository.Base;
using mbill_service.Core.Security;
using mbill_service.Core.Domains.Entities.Core;

namespace mbill_service.Infrastructure.Repository.Core
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
