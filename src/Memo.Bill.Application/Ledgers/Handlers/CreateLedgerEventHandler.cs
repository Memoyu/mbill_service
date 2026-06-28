using Memo.Bill.Domain.Events.Ledgers;

namespace Memo.Bill.Application.Ledgers.Handlers;

public class CreateLedgerEventHandler(
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo
    ) : INotificationHandler<CreateLedgerEvent>
{
    public async Task Handle(CreateLedgerEvent notification, CancellationToken cancellationToken)
    {
        // 插入账本用户
        var maxSort = await ledgerUserRepo.Select.Where(l => l.UserId == notification.UserId).MaxAsync(l => l.Sort, cancellationToken);
        var ledgerUser = await ledgerUserRepo.InsertAsync(new LedgerUser 
        {
            LedgerId = notification.LedgerId,
            UserId = notification.UserId,
            Sort = ++maxSort 
        }, cancellationToken);
        if (ledgerUser.Id <= 0) throw new ApplicationException("保存账本用户失败");
    }
}