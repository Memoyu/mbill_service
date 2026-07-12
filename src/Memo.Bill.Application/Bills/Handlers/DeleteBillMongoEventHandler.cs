using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;
using MongoDB.Driver;

namespace Memo.Bill.Application.Bills.Handlers;

public class DeleteBillMongoEventHandler(IBaseMongoRepository<BillingCollection> billMongoRepo) : INotificationHandler<DeleteBillEvent>
{
    public async Task Handle(DeleteBillEvent notification, CancellationToken cancellationToken)
    {
        // TODO Mongo数据操作后续处理
        //FilterDefinitionBuilder<BillingCollection> buildFilter = Builders<BillingCollection>.Filter;
        //var filter = buildFilter.Eq(a => a.BillId, notification.BillId);
        //var res = await billMongoRepo.DeleteOneAsync(filter, null, cancellationToken);
        //if (res?.IsAcknowledged != true) throw new ApplicationException("删除Mongo账单失败");
    }
}
