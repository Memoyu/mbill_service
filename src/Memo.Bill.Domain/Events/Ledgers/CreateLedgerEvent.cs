namespace Memo.Bill.Domain.Events.Ledgers;

public record CreateLedgerEvent(long LedgerId, long UserId, int Color) : IDomainEvent;