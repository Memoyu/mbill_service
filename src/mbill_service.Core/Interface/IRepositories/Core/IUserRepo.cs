using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using mbill_service.Core.Interface.IRepositories.Base;
using mbill_service.Core.Domains.Entities.User;

namespace mbill_service.Core.Interface.IRepositories.Core
{
    public interface IUserRepo : IAuditBaseRepo<UserEntity>
    {
        /// <summary>
        /// 根据条件得到用户信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<UserEntity> GetUserAsync(Expression<Func<UserEntity, bool>> expression);

        /// <summary>
        /// 根据用户Id更新用户的最后登录时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateLastLoginTimeAsync(long userId);
    }
}
