/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Repositories.Core
*   文件名称 ：PermissionRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-15 0:07:09
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Entities.Core;
using Memoyu.Mbill.Domain.IRepositories.Core;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.Core
{
    public class PermissionRepository : AuditBaseRepository<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
        }
    }
}
