using FreeSql.DataAnnotations;
using Mbill.Core.AOP.Attributes;
using Mbill.Core.Common.Configs;
using Mbill.Core.Extensions;
using Mbill.Core.Interface.IRepositories.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mbill.Infrastructure.Repository.Base;

public class MongoBaseRepo<T> : IMongoBaseRepo<T>
    where T : class, new()
{
    private readonly IMongoDatabase _database;

    private readonly string _collName;

    public MongoBaseRepo(MongoClient client)
    {
        _database = client.GetDatabase(Appsettings.MongoDBName);
        _collName = typeof(T).GetAttributeValue((MongoCollectionAttribute m) => m.Name);
        _collName = _collName ?? typeof(T).Name;
    }

    public async Task<bool> InsertOneAsync(T t)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            await client.InsertOneAsync(t);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> InsertManyAsync(List<T> t)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            await client.InsertManyAsync(t);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ReplaceOneResult> ReplaceOneAsync(T replacement, FilterDefinition<T> filter)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            return await client.ReplaceOneAsync(filter, replacement);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UpdateResult> UpdateOneAsync(UpdateDefinition<T> update, FilterDefinition<T> filter)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            return await client.UpdateOneAsync(filter, update);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UpdateResult> UpdateManayAsync(UpdateDefinition<T> update, FilterDefinition<T> filter)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            return await client.UpdateManyAsync(filter, update);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            return await client.DeleteOneAsync(filter);
        }
        catch (Exception)
        {
            throw;
        }

    }

    public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            return await client.DeleteManyAsync(filter);
        }
        catch (Exception)
        {
            throw;
        }

    }

    public async Task<T> FindOneAsync(long id, bool isObjectId = true, string[] field = null)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
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
                return await client.Find(filter).FirstOrDefaultAsync();
            }

            //制定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();
            return await client.Find(filter).Project<T>(projection).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<T>> FindListAsync(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                //return await client.Find(new BsonDocument()).ToListAsync();
                if (sort == null) return await client.Find(filter).ToListAsync();
                return await client.Find(filter).Sort(sort).ToListAsync();
            }

            //制定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();
            if (sort == null) return await client.Find(filter).Project<T>(projection).ToListAsync();
            //排序查询
            return await client.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageInd, int pageSize, string[] field = null, SortDefinition<T> sort = null)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                if (sort == null) return await client.Find(filter).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();
                //进行排序
                return await client.Find(filter).Sort(sort).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();
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
            if (sort == null) return await client.Find(filter).Project<T>(projection).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();

            //排序查询
            return await client.Find(filter).Sort(sort).Project<T>(projection).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync();

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<long> CountAsync(FilterDefinition<T> filter)
    {
        try
        {
            var client = _database.GetCollection<T>(_collName);
            return await client.CountDocumentsAsync(filter);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
