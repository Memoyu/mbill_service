using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;
using MongoDB.Driver;

namespace Memo.Bill.Application.Bills.Handlers;

internal class UpdateBillMongoEventHandler(IMapper mapper, IBaseMongoRepository<BillingCollection> billMongoRepo) : INotificationHandler<UpdateBillEvent>
{
    public async Task Handle(UpdateBillEvent notification, CancellationToken cancellationToken)
    {
        var update = mapper.Map<BillingCollection>(notification.Bill);

        var success = false;
        var bill = await billMongoRepo.FindOneAsync(notification.Bill.BillId, false);
        if (bill == null)
        {
            success = await billMongoRepo.InsertOneAsync(update, null, cancellationToken);   
        }
        else
        {
            var filter = Builders<BillingCollection>.Filter.Eq(b => b.BillId, update.BillId);
            var resMongo = await billMongoRepo.ReplaceOneAsync(update, filter, null, cancellationToken);
            success = resMongo?.IsAcknowledged ?? false;
        }
        if (!success) throw new ApplicationException("更新账单到Mongo失败");
    }
}
