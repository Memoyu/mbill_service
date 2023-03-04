using MongoDB.Driver;

namespace mbill.Infrastructure.Repository.Bill;

public class BillMongoRepo : MongoBaseRepo<BillEntity>, IBillMongoRepo
{
    public BillMongoRepo(MongoClient client) : base(client)
    {

    }
}
