namespace Memo.Bill.Domain.Events.Roles;

public record DeletedRoleEvent(long RoleId) : IDomainEvent;
