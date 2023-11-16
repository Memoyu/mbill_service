using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;

namespace Mbill.Infrastructure.Repository.Core
{
    public class IPLogRepo : AuditBaseRepo<IPLogEntity>, IIPLogRepo
    {
        public IPLogRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
