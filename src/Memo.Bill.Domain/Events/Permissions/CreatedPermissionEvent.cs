namespace Memo.Bill.Domain.Events.Permissions;

public record CreatedPermissionEvent(long PermissionId) : IDomainEvent;
