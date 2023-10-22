using MongoDB.Driver;

namespace Mbill.Infrastructure.Repository.Bill;

public class BillMongoRepo : MongoBaseRepo<BillEntity>, IBillMongoRepo
{
    public BillMongoRepo(MongoClient client) : base(client)
    {

    }
}
