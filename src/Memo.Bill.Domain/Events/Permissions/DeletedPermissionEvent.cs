namespace Memo.Bill.Domain.Events.Permissions;

public record DeletedPermissionEvent(long PermissionId) : IDomainEvent;
