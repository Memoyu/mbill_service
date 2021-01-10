/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.IRepositories.User
*   文件名称 ：IUserRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:06:06
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Mbill.Domain.Entities.User;
using Memoyu.Mbill.Domain.Base;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace Memoyu.Mbill.Domain.IRepositories.User
{
    public interface IUserRepository : IAuditBaseRepository<UserEntity>
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
