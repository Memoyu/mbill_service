using FreeSql;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.Domain.Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Repositories.Core
{
    public class RoleRepository : AuditBaseRepository<RoleEntity>, IRoleRepository
    {
        public RoleRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
