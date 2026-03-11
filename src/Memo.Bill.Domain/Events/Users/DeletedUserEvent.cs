namespace Memo.Bill.Domain.Events.Users;

public record DeletedUserEvent(long UserId) : IDomainEvent;
