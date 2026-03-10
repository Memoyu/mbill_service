namespace Memo.Bill.Domain.Events.Bills;

/// <summary>
/// 删除账单
/// </summary>
/// <param name="BillId">账单Id</param>
public record DeleteBillEvent(long BillId): IDomainEvent;
