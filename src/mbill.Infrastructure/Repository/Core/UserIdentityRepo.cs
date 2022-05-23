using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Core;
using mbill.Core.Security;
using mbill.Infrastructure.Repository.Base;
using FreeSql;

namespace mbill.Infrastructure.Repository.Core
{
    class UserIdentityRepo : AuditBaseRepo<UserIdentityEntity>, IUserIdentityRepo
    {
        public UserIdentityRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
