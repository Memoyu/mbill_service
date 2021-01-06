/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Repositories.User
*   文件名称 ：UserRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.IRepositories.User;
using Memoyu.Mbill.Domain.Base.Impl;
using Memoyu.Mbill.Domain.Shared.Security;

namespace Memoyu.Mbill.Domain.Repositories.User
{
    public class UserRepository : AuditBaseRepository<UserEntity>, IUserRepository
    {
        private readonly ICurrentUser _currentUser;
        public UserRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
