using Memo.Bill.Domain.Entities;

namespace Memo.Bill.Domain.Events.Bills;

/// <summary>
/// 更新账单事件
/// </summary>
/// <param name="Bill">账单实体</param>
public record UpdateBillEvent(Billing Bill) : IDomainEvent;