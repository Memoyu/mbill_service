using Mbill.Core.AOP.Attributes;
using Mbill.Core.Common.Configs;
using Mbill.Core.Extensions;
using Mbill.Core.Interface.IRepositories.Base;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mbill.Infrastructure.Repository.Base;

public class MongoBaseRepo<T> : IMongoBaseRepo<T>
    where T : class, new()
{
    private readonly ILogger _logger;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<T> _collection;
    private readonly string _collName;

    public MongoBaseRepo(ILoggerFactory loggerFactory, MongoClient client)
    {
        _logger = loggerFactory.CreateLogger<MongoBaseRepo<T>>();
        _database = client.GetDatabase(Appsettings.MongoDBName);
        _collName = typeof(T).GetAttributeValue((MongoCollectionAttribute m) => m.Name);
        _collName = _collName ?? typeof(T).Name;
        _collection = _database.GetCollection<T>(_collName) ?? throw new ArgumentNullException($"The MongoDB collection named '{_collName}' is null ");
    }

    public async Task<bool> InsertOneAsync(T t)
    {
        try
        {
            await _collection.InsertOneAsync(t);
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"{nameof(InsertOneAsync)} 异常");
            return false;
        }
    }

    public async Task<bool> InsertManyAsync(List<T> t)
    {
        try
        {
            await _collection.InsertManyAsync(t);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(InsertManyAsync)} 异常");
            return false;
        }
    }

    public async Task<ReplaceOneResult> ReplaceOneAsync(T replacement, FilterDefinition<T> filter)
    {
        try
        {
            return await _collection.ReplaceOneAsync(filter, replacement);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(ReplaceOneAsync)} 异常");
            throw;
        }
    }

    public async Task<UpdateResult> UpdateOneAsync(UpdateDefinition<T> update, FilterDefinition<T> filter)
    {
        try
        {
            return await _collection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(UpdateOneAsync)} 异常");
            throw;
        }
    }

    public async Task<UpdateResult> UpdateManayAsync(UpdateDefinition<T> update, FilterDefinition<T> filter)
    {
        try
        {
            return await _collection.UpdateManyAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(UpdateManayAsync)} 异常");
            throw;
        }
    }

    public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter)
    {
        try
        {
            return await _collection.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(DeleteOneAsync)} 异常");
            throw;
        }

    }

    public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter)
    {
        try
        {
            return await _collection.DeleteManyAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(DeleteManyAsync)} 异常");
            throw;
        }

    }

    public async Task<T> FindOneAsync(long id, bool isObjectId = true, string[] field = null)
    {
        try
        {
            FilterDefinition<T> filter;
            if (isObjectId)
            {
                filter = Builders<T>.Filter.Eq("_id", new ObjectId(id.ToString()));
            }
            else
            {
                filter = Builders<T>.Filter.Eq("_id", id);
            }

            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                return await _collection.Find(filter).FirstOrDefaultAsync();
            }

            //制定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();
            return await _collection.Find(filter).Project<T>(projection).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(FindOneAsync)} 异常");
            throw;
        }
    }

    public async Task<List<T>> FindListAsync(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null)
    {
        try
        {
            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                //return await client.Find(new BsonDocument()).ToListAsync();
                if (sort == null) return await _collection.Find(filter).ToListAsync();
                return await _collection.Find(filter).Sort(sort).ToListAsync();
            }

            //制定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();
            if (sort == null) return await _collection.Find(filter).Project<T>(projection).ToListAsync();
            //排序查询
            return await _collection.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(FindListAsync)} 异常");
            throw;
        }
    }

    public async Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageInd, int pageSize, string[] field = null, SortDefinition<T> sort = null)
    {
        try
        {
            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                if (sort == null) return await _collection.Find(filter).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();
                //进行排序
                return await _collection.Find(filter).Sort(sort).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();
            }

            //制定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();

            //不排序
            if (sort == null) return await _collection.Find(filter).Project<T>(projection).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();

            //排序查询
            return await _collection.Find(filter).Sort(sort).Project<T>(projection).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(FindListByPageAsync)} 异常");
            throw;
        }
    }

    public async Task<long> CountAsync(FilterDefinition<T> filter)
    {
        try
        {
            return await _collection.CountDocumentsAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(CountAsync)} 异常");
            throw;
        }
    }
}
