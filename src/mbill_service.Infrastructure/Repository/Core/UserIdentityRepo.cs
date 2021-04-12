using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Interface.IRepositories.Core;
using mbill_service.Core.Security;
using mbill_service.Infrastructure.Repository.Base;
using FreeSql;

namespace mbill_service.Infrastructure.Repository.Core
{
    class UserIdentityRepo : AuditBaseRepo<UserIdentityEntity>, IUserIdentityRepo
    {
        public UserIdentityRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
