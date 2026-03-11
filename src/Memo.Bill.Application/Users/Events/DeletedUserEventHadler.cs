using Memo.Bill.Domain.Events.Users;

namespace Memo.Bill.Application.Users.Events;

public class DeletedUserEventHadler() : INotificationHandler<DeletedUserEvent>
{
    public Task Handle(DeletedUserEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
