using Memo.Bill.Domain.Events.Permissions;

namespace Memo.Bill.Application.Permissions.Events;

public class CreatedPermissionEventHadler(
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : INotificationHandler<CreatedPermissionEvent>
{
    public async Task Handle(CreatedPermissionEvent notification, CancellationToken cancellationToken)
    {
        // 添加管理员角色关联权限
        var permission = await rolePermissionRepo.InsertAsync(new RolePermission
        {
            RoleId = InitConst.InitAdminRoleId,
            PermissionId = notification.PermissionId,
        } ,cancellationToken);
    }
}
