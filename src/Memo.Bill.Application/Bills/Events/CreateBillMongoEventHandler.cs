using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Events;

public class CreateBillMongoEventHandler : INotificationHandler<CreateBillEvent>
{
    public Task Handle(CreateBillEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
