using FreeSql;

namespace Memo.Bill.Application.Common.Extensions;
public static class PersistenceExtensions
{
    /// <summary>
    /// 分页处理，同步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="pagination"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<TEntity> ToPageList<TEntity>(this ISelect<TEntity> source, PaginationQuery pagination, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pagination.Page, pagination.Size).ToList();
    }

    /// <summary>
    /// 分页处理，异步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="pagination"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Task<List<TEntity>> ToPageListAsync<TEntity>(this ISelect<TEntity> source, PaginationQuery pagination, out long count, CancellationToken cancellationToken = default) where TEntity : class
    {
        return source.Count(out count).Page(pagination.Page, pagination.Size).ToListAsync(true, cancellationToken);
    }

    /// <summary>
    /// 分页处理并映射Dto，同步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="pagination"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<TResult> ToPageList<TEntity, TResult>(this ISelect<TEntity> source, PaginationQuery pagination, out long count) where TEntity : class
    {
        return source.Count(out count).Page(pagination.Page, pagination.Size).ToList<TResult>();
    }

    /// <summary>
    /// 分页处理并映射Dto，异步
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="pagination"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Task<List<TResult>> ToPageListAsync<TEntity, TResult>(this ISelect<TEntity> source, PaginationQuery pagination, out long count, CancellationToken cancellationToken = default) where TEntity : class
    {
        return source.Count(out count).Page(pagination.Page, pagination.Size).ToListAsync<TResult>(cancellationToken);
    }

}
