/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Domain.Base
*   文件名称 ：IAuditBaseRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 16:00:54
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Memoyu.Mbill.Domain.Base
{
    public interface IAuditBaseRepository<TEntity> : IBaseRepository<TEntity, long> where TEntity : class
    {
        Task<int> UpdateWithIgnoreAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreExp, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IAuditBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        Task<int> UpdateWithIgnoreAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreExp, CancellationToken cancellationToken = default(CancellationToken));
    }
}
