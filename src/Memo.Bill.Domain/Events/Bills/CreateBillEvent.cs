using Memo.Bill.Domain.Entities;

namespace Memo.Bill.Domain.Events.Bills;

/// <summary>
/// 创建账单事件
/// </summary>
/// <param name="Bill">账单实体</param>
public record CreateBillEvent(Billing Bill) : IDomainEvent;
