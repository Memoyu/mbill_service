using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Events;

public class CreateBillMongoEventHandler(IMapper mapper, IBaseMongoRepository<BillingCollection> billMongoRepo) : INotificationHandler<CreateBillEvent>
{
    public async Task Handle(CreateBillEvent notification, CancellationToken cancellationToken)
    {
        var res = await billMongoRepo.InsertOneAsync(mapper.Map<BillingCollection>(notification.Bill), null, cancellationToken);
        if (!res) throw new ApplicationException("保存账单到Mongo失败");
    }
}
