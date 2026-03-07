using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Memo.Bill.Infrastructure.Persistence.Repositories;

public class BaseMongoRepository<TEntity> : IBaseMongoRepository<TEntity>
    where TEntity : class, new()
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<TEntity> _collection;

    public BaseMongoRepository(IOptions<MongoOptions> options, MongoClient client)
    {
        var mongoOptions = options.Value;
        _database = client.GetDatabase(mongoOptions.Database);
        _collection = GetCollection();
    }

    public async Task<bool> InsertOneAsync(TEntity t, InsertOneOptions? options, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(t, options, cancellationToken);
        return true;
    }

    public async Task<bool> InsertManyAsync(List<TEntity> t, InsertManyOptions? options, CancellationToken cancellationToken)
    {
        await _collection.InsertManyAsync(t, options, cancellationToken);
        return true;
    }

    public async Task<ReplaceOneResult> ReplaceOneAsync(TEntity replacement, FilterDefinition<TEntity> filter, ReplaceOptions? options, CancellationToken cancellationToken)
    {
        return await _collection.ReplaceOneAsync(filter, replacement, options, cancellationToken);
    }

    public async Task<UpdateResult> UpdateOneAsync(UpdateDefinition<TEntity> update, FilterDefinition<TEntity> filter, UpdateOptions? options, CancellationToken cancellationToken)
    {
        return await _collection.UpdateOneAsync(filter, update, options, cancellationToken);
    }

    public async Task<UpdateResult> UpdateManayAsync(UpdateDefinition<TEntity> update, FilterDefinition<TEntity> filter, UpdateOptions? options, CancellationToken cancellationToken)
    {
        return await _collection.UpdateManyAsync(filter, update, options, cancellationToken);
    }

    public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<TEntity> filter, DeleteOptions? options, CancellationToken cancellationToken)
    {
        return await _collection.DeleteOneAsync(filter, options, cancellationToken);
    }

    public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<TEntity> filter, DeleteOptions? options, CancellationToken cancellationToken)
    {
        return await _collection.DeleteManyAsync(filter, options, cancellationToken);
    }

    public async Task<TEntity> FindOneAsync(long id, bool isObjectId = true, string[]? field = null, CancellationToken cancellationToken = default)
    {
        FilterDefinition<TEntity> filter;
        if (isObjectId)
        {
            filter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id.ToString()));
        }
        else
        {
            filter = Builders<TEntity>.Filter.Eq("_id", id);
        }

        //不指定查询字段
        if (field == null || field.Length == 0)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        //制定查询字段
        var fieldList = new List<ProjectionDefinition<TEntity>>();
        for (int i = 0; i < field.Length; i++)
        {
            fieldList.Add(Builders<TEntity>.Projection.Include(field[i].ToString()));
        }
        var projection = Builders<TEntity>.Projection.Combine(fieldList);
        fieldList?.Clear();
        return await _collection.Find(filter).Project<TEntity>(projection).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TEntity>> FindListAsync(FilterDefinition<TEntity> filter, string[]? field = null, SortDefinition<TEntity>? sort = null, CancellationToken cancellationToken = default)
    {
        //不指定查询字段
        if (field == null || field.Length == 0)
        {
            //return await client.Find(new BsonDocument()).ToListAsync();
            if (sort == null) return await _collection.Find(filter).ToListAsync();
            return await _collection.Find(filter).Sort(sort).ToListAsync(cancellationToken);
        }

        //制定查询字段
        var fieldList = new List<ProjectionDefinition<TEntity>>();
        for (int i = 0; i < field.Length; i++)
        {
            fieldList.Add(Builders<TEntity>.Projection.Include(field[i].ToString()));
        }
        var projection = Builders<TEntity>.Projection.Combine(fieldList);
        fieldList?.Clear();
        if (sort == null) return await _collection.Find(filter).Project<TEntity>(projection).ToListAsync(cancellationToken);
        //排序查询
        return await _collection.Find(filter).Sort(sort).Project<TEntity>(projection).ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> FindListByPageAsync(FilterDefinition<TEntity> filter, int pageInd, int pageSize, string[]? field = null, SortDefinition<TEntity>? sort = null, CancellationToken cancellationToken = default)
    {
        //不指定查询字段
        if (field == null || field.Length == 0)
        {
            if (sort == null) return await _collection.Find(filter).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
            //进行排序
            return await _collection.Find(filter).Sort(sort).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
        }

        //制定查询字段
        var fieldList = new List<ProjectionDefinition<TEntity>>();
        for (int i = 0; i < field.Length; i++)
        {
            fieldList.Add(Builders<TEntity>.Projection.Include(field[i].ToString()));
        }
        var projection = Builders<TEntity>.Projection.Combine(fieldList);
        fieldList?.Clear();

        //不排序
        if (sort == null) return await _collection.Find(filter).Project<TEntity>(projection).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);

        //排序查询
        return await _collection.Find(filter).Sort(sort).Project<TEntity>(projection).Skip((pageInd - 1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<long> CountAsync(FilterDefinition<TEntity> filter, CountOptions? options, CancellationToken cancellationToken)
    {
        return await _collection.CountDocumentsAsync(filter, options, cancellationToken);
    }

    private IMongoCollection<TEntity> GetCollection()
    {
        var type = typeof(TEntity);
        var collectionName = type.Name;
        var att = type.GetCustomAttributes(typeof(MongoCollectionAttribute), true).FirstOrDefault() as MongoCollectionAttribute;
        if (att is not null && !string.IsNullOrWhiteSpace(att.Name))
        {
            collectionName = att.Name;
        }

        return _database.GetCollection<TEntity>(collectionName) ?? throw new ArgumentNullException($"The MongoDB collection named '{collectionName}' is null "); ;
    }
}
