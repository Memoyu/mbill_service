
using FreeSql;
using mbill_service.Core.Domains.Entities.Bill;
using mbill_service.Core.Interface.IRepositories.Bill;
using mbill_service.Core.Security;
using mbill_service.Infrastructure.Repository.Base;

namespace mbill_service.Infrastructure.Repository.Bill
{
    public class StatementRepo : AuditBaseRepo<StatementEntity>, IStatementRepo
    {
        private readonly ICurrentUser _currentUser;
        public StatementRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
