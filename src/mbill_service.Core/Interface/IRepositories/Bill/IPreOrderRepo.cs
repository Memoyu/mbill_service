using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Core.Interface.IRepositories.Base;

namespace mbill_service.Core.Interface.IRepositories.Bill
{
    public interface IPreOrderRepo : IAuditBaseRepo<PreOrderEntity>
    {
    }
}
