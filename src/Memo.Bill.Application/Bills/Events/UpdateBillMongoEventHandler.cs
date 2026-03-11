using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;
using MongoDB.Driver;

namespace Memo.Bill.Application.Bills.Events;

internal class UpdateBillMongoEventHandler(IMapper mapper, IBaseMongoRepository<BillingCollection> billMongoRepo) : INotificationHandler<UpdateBillEvent>
{
    public async Task Handle(UpdateBillEvent notification, CancellationToken cancellationToken)
    {
        var bill = await billMongoRepo.FindOneAsync(notification.Bill.BillId, false);
        if (bill == null)
        {
            var res = await billMongoRepo.InsertOneAsync(mapper.Map<BillingCollection>(notification.Bill), null, cancellationToken);
            if (!res) throw new ApplicationException("更新账单到Mongo失败");
        }
        else
        {
            var nbill = notification.Bill;
            var update = Builders<BillingCollection>.Update
               .Set(nameof(BillingCollection.CategoryId), nbill.CategoryId)
               .Set(nameof(BillingCollection.AccountId), nbill.AccountId)
               .Set(nameof(BillingCollection.Amount), nbill.Amount)
               .Set(nameof(BillingCollection.Type), nbill.Type)
               .Set(nameof(BillingCollection.Remark), nbill.Remark)
               .Set(nameof(BillingCollection.Location), nbill.Location)
               .Set(nameof(BillingCollection.Address), nbill.Address)
               .Set(nameof(BillingCollection.Date), nbill.Date);
            var filter = Builders<BillingCollection>.Filter.Eq(b => b.BillId, nbill.BillId);
            var resMongo = await billMongoRepo.UpdateOneAsync(update, filter, null, cancellationToken)
                ?? throw new ApplicationException("更新失败");
        }
    }
}
