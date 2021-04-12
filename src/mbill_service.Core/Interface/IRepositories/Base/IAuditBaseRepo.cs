using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace mbill_service.Core.Interface.IRepositories.Base
{
    public interface IAuditBaseRepo<TEntity> : IBaseRepository<TEntity, long> where TEntity : class
    {
        Task<int> UpdateWithIgnoreAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreExp, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IAuditBaseRepo<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        Task<int> UpdateWithIgnoreAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreExp, CancellationToken cancellationToken = default(CancellationToken));
    }
}
