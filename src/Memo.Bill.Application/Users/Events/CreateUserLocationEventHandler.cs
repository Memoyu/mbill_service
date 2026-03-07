using Memo.Bill.Domain.Events.Aggregations;

namespace Memo.Bill.Application.Users.Events;

public class CreateUserLocationEventHandler(
    IBaseDefaultRepository<UserLocation> userLocationRepo
    ) : INotificationHandler<UserGetLocationEvent>
{
    public async Task Handle(UserGetLocationEvent notification, CancellationToken cancellationToken)
    {
        // 插入用户位置信息
        await userLocationRepo.InsertAsync(notification.UserLocation, cancellationToken);
    }
}
