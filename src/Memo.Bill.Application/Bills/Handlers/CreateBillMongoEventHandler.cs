using Memo.Bill.Domain.Entities.Mongo;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Handlers;

public class CreateBillMongoEventHandler(
    IMapper mapper,
    IBaseMongoRepository<BillingCollection> billMongoRepo,
    IBaseDefaultRepository<FrequencyRecord> frequencyRecordRepo) : INotificationHandler<CreateBillEvent>
{
    public async Task Handle(CreateBillEvent notification, CancellationToken cancellationToken)
    {
        var bill = notification.Bill;
        var success = await billMongoRepo.InsertOneAsync(mapper.Map<BillingCollection>(bill), null, cancellationToken);
        if (!success) throw new ApplicationException("保存账单到Mongo失败");

        // 记录分类使用记录
        var caRecord = await frequencyRecordRepo.Select.Where(r => r.RecordId == bill.CategoryId && r.Type == FrequencyRecordType.Category).FirstAsync(cancellationToken)
            ?? new FrequencyRecord { RecordId = bill.CategoryId, Type = FrequencyRecordType.Category };
        caRecord.Frequency += 1;
        await frequencyRecordRepo.InsertOrUpdateAsync(caRecord, cancellationToken);

        // 记录账户使用记录
        var acRecord = await frequencyRecordRepo.Select.Where(r => r.RecordId == bill.AccountId && r.Type == FrequencyRecordType.Account).FirstAsync(cancellationToken)
            ?? new FrequencyRecord { RecordId = bill.AccountId, Type = FrequencyRecordType.Account };
        acRecord.Frequency += 1;
        await frequencyRecordRepo.InsertOrUpdateAsync(acRecord, cancellationToken);

        // 记录标签使用记录
        foreach (var tag in bill.Tags)
        {
            var tagRecord = await frequencyRecordRepo.Select.Where(r => r.RecordId == tag.TagId && r.Type == FrequencyRecordType.Tag).FirstAsync(cancellationToken)
                ?? new FrequencyRecord { RecordId = tag.TagId, Type = FrequencyRecordType.Tag };
            tagRecord.Frequency += 1;
            await frequencyRecordRepo.InsertOrUpdateAsync(tagRecord, cancellationToken);
        }
    }
}
