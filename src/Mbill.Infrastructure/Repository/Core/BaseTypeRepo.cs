using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;
using Mbill.Core.Security;
using Mbill.Infrastructure.Repository.Base;
using FreeSql;

namespace Mbill.Infrastructure.Repository.Core
{
    public class BaseTypeRepo : AuditBaseRepo<BaseTypeEntity>, IBaseTypeRepo
    {
        public BaseTypeRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
