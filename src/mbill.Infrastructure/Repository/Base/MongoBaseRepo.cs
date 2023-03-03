using mbill.Core.Common.Configs;
using MongoDB.Driver;

namespace mbill.Infrastructure.Repository.Base;

public class MongoBaseRepo<T> where T : class, new()
{
    private readonly IMongoDatabase _database;

    private readonly string _collName;

    public MongoBaseRepo(MongoClient client)
    {
        _database = client.GetDatabase(Appsettings.MongoDBName);
        //_collName = typeof(T).GetAttributeValue((TableAttribute m) => m.Name);
        //_collName = _collName ?? typeof(T).Name;

    }

}
