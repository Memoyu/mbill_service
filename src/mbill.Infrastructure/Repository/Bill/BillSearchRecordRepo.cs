using MongoDB.Driver;

namespace mbill.Infrastructure.Repository.Bill;

public class BillSearchRecordRepo : MongoBaseRepo<BillSearchRecordEntity>, IBillSearchRecordRepo
{
    public BillSearchRecordRepo(MongoClient client) : base(client)
    {

    }
}
