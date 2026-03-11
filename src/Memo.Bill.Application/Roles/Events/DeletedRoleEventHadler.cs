using Memo.Bill.Domain.Events.Roles;

namespace Memo.Bill.Application.Roles.Events;

public class DeletedRoleEventHadler(
    IBaseDefaultRepository<RolePermission> rolePermissionRepo,
    IBaseDefaultRepository<UserRole> userRoleRepo
    ) : INotificationHandler<DeletedRoleEvent>
{
    public async Task Handle(DeletedRoleEvent notification, CancellationToken cancellationToken)
    {
        // 删除关联权限
        var permissionAffrows = await rolePermissionRepo.DeleteAsync(rp => rp.RoleId == notification.RoleId, cancellationToken);

        // 删除用户关联
        var userRoleAffrows = await userRoleRepo.DeleteAsync(ur => ur.RoleId == notification.RoleId, cancellationToken);
    }
}
