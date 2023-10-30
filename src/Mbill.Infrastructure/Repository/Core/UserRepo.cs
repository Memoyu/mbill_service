using System.Linq.Expressions;
using Mbill.Core.Interface.IRepositories.Core;
using Mbill.Core.Domains.Entities.User;

namespace Mbill.Infrastructure.Repository.Core
{
    public class UserRepo : AuditBaseRepo<UserEntity>, IUserRepo
    {
        private readonly ICurrentUser _currentUser;
        public UserRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }

        public Task<UserEntity> GetUserAsync(long bId)
        {
            return Select.Where(u=> u.BId == bId).ToOneAsync();
        }

        public Task<UserEntity> GetUserAsync(Expression<Func<UserEntity, bool>> expression)
        {
            return Select.Where(expression).ToOneAsync();
        }

        public Task UpdateLastLoginTimeAsync(long userId)
        {
            return UpdateDiy.Set(r => new UserEntity()
            {
                LastLoginTime = DateTime.Now
            }).Where(r => r.Id == userId).ExecuteAffrowsAsync();
        }
    }
}
