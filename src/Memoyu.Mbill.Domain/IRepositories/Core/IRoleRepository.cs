using Memoyu.Mbill.Domain.Base;
using Memoyu.Mbill.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.IRepositories.Core
{
    public interface IRoleRepository : IAuditBaseRepository<RoleEntity>
    {
    }
}
