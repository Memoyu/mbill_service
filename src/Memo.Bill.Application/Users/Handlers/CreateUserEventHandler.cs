using Memo.Bill.Domain.Events.Ledgers;
using Memo.Bill.Domain.Events.Users;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace Memo.Bill.Application.Users.Handlers;

public class CreateUserEventHandler(
    IMapper mapper,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : INotificationHandler<CreateUserEvent>
{
    public async Task Handle(CreateUserEvent notification, CancellationToken cancellationToken)
    {
        // 新增默认账本
        var ledgerId = SnowFlakeUtil.NextId();
        var ledger = new Ledger
        {
            LedgerId = ledgerId,
            Name = "日常账本",
            Default = true,
            CreateUserId = notification.UserId,
            UpdateUserId = notification.UserId,
        };
        ledger.AddDomainEvent(new CreateLedgerEvent(ledgerId, notification.UserId, 0));
        await ledgerRepo.InsertAsync(ledger, cancellationToken);

        // 初始化分类
        var defCategories = await categoryRepo.Select.Where(c => c.Default).ToListAsync(cancellationToken);
        var categories = new List<Category>();
        foreach (var dpc in defCategories.Where(c => c.ParentId is null))
        {
            var p = ToNewCategory(dpc);
            categories.Add(p);
            categories.AddRange(defCategories.Where(c => c.ParentId == dpc.CategoryId).Select(a => ToNewCategory(a, p.CategoryId)));

            Category ToNewCategory(Category c, long? parentId = null)
            {
                var nc = mapper.Map<Category>(c);
                nc.Id = 0;
                nc.CategoryId = SnowFlakeUtil.NextId();
                nc.ParentId = parentId;
                nc.Default = false;
                nc.CreateUserId = notification.UserId;
                nc.UpdateUserId = notification.UserId;
                return nc;
            }
        }
        await categoryRepo.InsertAsync(categories, cancellationToken);


        // 初始化账户
        var defAccounts = await accountRepo.Select.Where(a => a.Default).ToListAsync(cancellationToken);
        var accounts = new List<Account>();
        foreach (var dpa in defAccounts.Where(c => c.ParentId is null))
        {
            var p = ToNewAccount(dpa);
            accounts.Add(p);
            accounts.AddRange(defAccounts.Where(c => c.ParentId == dpa.AccountId).Select(a => ToNewAccount(a, p.AccountId)));

            Account ToNewAccount(Account a, long? parentId = null)
            {
                var na = mapper.Map<Account>(a);
                na.Id = 0;
                na.AccountId = SnowFlakeUtil.NextId();
                na.ParentId = parentId;
                na.Default = false;
                na.CreateUserId = notification.UserId;
                na.UpdateUserId = notification.UserId;

                return na;
            }
        }
        await accountRepo.InsertAsync(accounts, cancellationToken);
    }
}
