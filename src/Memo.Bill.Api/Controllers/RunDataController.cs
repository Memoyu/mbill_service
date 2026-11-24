using Memo.Bill.Application.Common.Interfaces.Persistence.Repositories;
using Memo.Bill.Domain.Entities;
using Memo.Bill.Domain.Entities.Mongo;

namespace Memo.Bill.Api.Controllers
{
    [AllowAnonymous]
    public class RunDataController(IPublisher publisher, IBaseDefaultRepository<Billing> billRepo, IBaseMongoRepository<BillingCollection> billMongoRepo) : ApiControllerBase
    {
        
    }
}
