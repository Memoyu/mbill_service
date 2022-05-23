
using FreeSql;
using mbill.Core.Domains.Entities.Bill;
using mbill.Core.Interface.IRepositories.Bill;
using mbill.Core.Security;
using mbill.Infrastructure.Repository.Base;

namespace mbill.Infrastructure.Repository.Bill
{
    public class PreOrderRepo : AuditBaseRepo<PreOrderEntity>, IPreOrderRepo
    {
        private readonly ICurrentUser _currentUser;
        public PreOrderRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
