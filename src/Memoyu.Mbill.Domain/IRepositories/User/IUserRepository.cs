using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.Base;

namespace Memoyu.Mbill.Domain.IRepositories.User
{
    public interface IUserRepository : IAuditBaseRepository<UserEntity>
    {
    }
}
