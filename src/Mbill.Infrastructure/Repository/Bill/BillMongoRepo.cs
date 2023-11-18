using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Mbill.Infrastructure.Repository.Bill;

public class BillMongoRepo : MongoBaseRepo<BillEntity>, IBillMongoRepo
{
    public BillMongoRepo(ILoggerFactory loggerFactory, MongoClient client) : base(loggerFactory, client)
    {

    }
}
