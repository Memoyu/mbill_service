using Memo.Bill.Domain.Events.Ledgers;
using Memo.Bill.Domain.Events.Users;

namespace Memo.Bill.Application.Users.Handlers;
public class CreateUserEventHandler(IBaseDefaultRepository<Ledger> ledgerRepo) : INotificationHandler<CreateUserEvent>
{
    public async Task Handle(CreateUserEvent notification, CancellationToken cancellationToken)
    {
        // 新增默认账本
        var ledgerId = SnowFlakeUtil.NextId();
        var ledger = new Ledger
        {
            LedgerId = ledgerId,
            Name = "日常账本",
            Color = 0,
            Default = true,
            CreateUserId = notification.UserId,
        };
        ledger.AddDomainEvent(new CreateLedgerEvent(ledgerId, notification.UserId));
        await ledgerRepo.InsertAsync(ledger, cancellationToken);
    }
}
