using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Core.Interface.IRepositories.Base;

namespace mbill_service.Core.Interface.IRepositories.Core
{
    public interface IPermissionRepo: IAuditBaseRepo<PermissionEntity>
    {
    }
}
