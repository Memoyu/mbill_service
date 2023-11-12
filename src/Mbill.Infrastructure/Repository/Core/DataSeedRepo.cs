using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;

namespace Mbill.Infrastructure.Repository.Core;

public class DataSeedRepo : AuditBaseRepo<DataSeedEntity>, IDataSeedRepo
{
    public DataSeedRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }
}
