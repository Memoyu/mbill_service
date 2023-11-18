using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Mbill.Infrastructure.Repository.Bill;

public class BillSearchRecordRepo : MongoBaseRepo<BillSearchRecordEntity>, IBillSearchRecordRepo
{
    public BillSearchRecordRepo(ILoggerFactory loggerFactory, MongoClient client) : base(loggerFactory, client)
    {

    }
}
