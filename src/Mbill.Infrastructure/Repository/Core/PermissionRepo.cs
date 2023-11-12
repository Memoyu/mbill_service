using Mbill.Core.Domains.Entities.Core;
using Mbill.Core.Interface.IRepositories.Core;

namespace Mbill.Infrastructure.Repository.Core;

public class PermissionRepo : AuditBaseRepo<PermissionEntity>, IPermissionRepo
{
    public PermissionRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }
}
