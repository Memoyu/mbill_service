using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Handlers;

public class CreateBillMongoEventHandler(IMapper mapper, IBaseMongoRepository<BillingCollection> billMongoRepo) : INotificationHandler<CreateBillEvent>
{
    public async Task Handle(CreateBillEvent notification, CancellationToken cancellationToken)
    {
        var success = await billMongoRepo.InsertOneAsync(mapper.Map<BillingCollection>(notification.Bill), null, cancellationToken);
        if (!success) throw new ApplicationException("保存账单到Mongo失败");
    }
}
