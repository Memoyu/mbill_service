using FreeSql;
using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Core;
using mbill.Core.Security;
using mbill.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbill.Infrastructure.Repository.Core
{
    public class LogRepo : AuditBaseRepo<LogEntity>, ILogRepo
    {
        public LogRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
