using FreeSql;
using Memoyu.Mbill.Domain.Entities.Statement;
using Memoyu.Mbill.Domain.IRepositories.Statement;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.Statement
{
    public class StatementRepository : AuditBaseRepository<StatementEntity>, IStatementRepository
    {
        private readonly ICurrentUser _currentUser;
        public StatementRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
