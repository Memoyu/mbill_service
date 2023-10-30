using MongoDB.Bson;
using MongoDB.Driver;

namespace Mbill.Core.Interface.IRepositories.Base;

public interface IMongoBaseRepo<T> : IScopedDependency
   where T : class, new()
{
    /// <summary>
    /// 异步添加一条数据
    /// </summary>
    /// <param name="t">添加的实体</param>
    /// <returns></returns>
    Task<bool> InsertOneAsync(T t);

    /// <summary>
    /// 异步批量插入
    /// </summary>
    /// <param name="t">实体集合</param>
    /// <returns></returns>
    Task<bool> InsertManyAsync(List<T> t);

    /// <summary>
    /// 异步替换修改数据
    /// </summary>
    /// <param name="replacement">要替换的实体</param>
    /// <param name="filter">替换条件</param>
    /// <returns></returns>
    Task<ReplaceOneResult> ReplaceOneAsync(T replacement, FilterDefinition<T> filter);

    /// <summary>
    /// 异步批量修改数据
    /// </summary>
    /// <param name="update">要修改的字段</param>
    /// <param name="filter">修改条件</param>
    /// <returns></returns>
    Task<UpdateResult> UpdateOneAsync(UpdateDefinition<T> update, FilterDefinition<T> filter);

    /// <summary>
    /// 异步批量修改数据
    /// </summary>
    /// <param name="update">要修改的字段</param>
    /// <param name="filter">修改条件</param>
    /// <returns></returns>
    Task<UpdateResult> UpdateManayAsync(UpdateDefinition<T> update, FilterDefinition<T> filter);

    /// <summary>
    /// 异步删除多条数据
    /// </summary>
    /// <param name="filter">删除的条件</param>
    /// <returns></returns>
    Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter);

    /// <summary>
    /// 异步删除多条数据
    /// </summary>
    /// <param name="filter">删除的条件</param>
    /// <returns></returns>
    Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter);

    /// <summary>
    /// 异步根据id查询一条数据
    /// </summary>
    /// <param name="id">objectid</param>
    /// <returns></returns>
    Task<T> FindOneAsync(long id, bool isObjectId = true, string[] field = null);

    /// <summary>
    /// 异步查询集合
    /// </summary>
    /// <param name="filter">查询条件</param>
    /// <param name="field">要查询的字段,不写时查询全部</param>
    /// <param name="sort">要排序的字段</param>
    /// <returns></returns>
    Task<List<T>> FindListAsync(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null);

    /// <summary>
    /// 异步分页查询集合
    /// </summary>
    /// <param name="filter">查询条件</param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">页容量</param>
    /// <param name="field">要查询的字段,不写时查询全部</param>
    /// <param name="sort">要排序的字段</param>
    /// <returns></returns>
    Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null);

    /// <summary>
    /// 异步根据条件获取总数
    /// </summary>
    /// <param name="filter">条件</param>
    /// <returns></returns>
    Task<long> CountAsync(FilterDefinition<T> filter);
}
