using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Handlers;

public class CreateBillMongoEventHandler(
    IMapper mapper,
    IBaseMongoRepository<BillingCollection> billMongoRepo,
    IBaseDefaultRepository<BillPropUsageRecord> billPropUsageRepo) : INotificationHandler<CreateBillEvent>
{
    public async Task Handle(CreateBillEvent notification, CancellationToken cancellationToken)
    {
        var bill = notification.Bill;
        var success = await billMongoRepo.InsertOneAsync(mapper.Map<BillingCollection>(bill), null, cancellationToken);
        if (!success) throw new ApplicationException("保存账单到Mongo失败");

        // 记录分类使用记录
        var caRecord = await billPropUsageRepo.Select.Where(r => r.RecordId == bill.CategoryId && r.RecordType == BillPropUsageRecordType.Category).FirstAsync(cancellationToken)
            ?? new BillPropUsageRecord { RecordId = bill.CategoryId, RecordType = BillPropUsageRecordType.Category, Type = bill.Category.Type };
        caRecord.Frequency += 1;
        await billPropUsageRepo.InsertOrUpdateAsync(caRecord, cancellationToken);

        // 记录账户使用记录
        var acRecord = await billPropUsageRepo.Select.Where(r => r.RecordId == bill.AccountId && r.RecordType == BillPropUsageRecordType.Account).FirstAsync(cancellationToken)
            ?? new BillPropUsageRecord { RecordId = bill.AccountId, RecordType = BillPropUsageRecordType.Account };
        acRecord.Frequency += 1;
        await billPropUsageRepo.InsertOrUpdateAsync(acRecord, cancellationToken);

        // 记录标签使用记录
        foreach (var tag in bill.Tags)
        {
            var tagRecord = await billPropUsageRepo.Select.Where(r => r.RecordId == tag.TagId && r.RecordType == BillPropUsageRecordType.Tag).FirstAsync(cancellationToken)
                ?? new BillPropUsageRecord { RecordId = tag.TagId, RecordType = BillPropUsageRecordType.Tag };
            tagRecord.Frequency += 1;
            await billPropUsageRepo.InsertOrUpdateAsync(tagRecord, cancellationToken);
        }
    }
}
