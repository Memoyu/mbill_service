namespace Memo.Bill.Domain.Events.Users;

public record CreateUserEvent(long UserId) : IDomainEvent;