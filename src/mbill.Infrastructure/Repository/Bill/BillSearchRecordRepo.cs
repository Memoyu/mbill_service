using MongoDB.Driver;

namespace Mbill.Infrastructure.Repository.Bill;

public class BillSearchRecordRepo : MongoBaseRepo<BillSearchRecordEntity>, IBillSearchRecordRepo
{
    public BillSearchRecordRepo(MongoClient client) : base(client)
    {

    }
}
