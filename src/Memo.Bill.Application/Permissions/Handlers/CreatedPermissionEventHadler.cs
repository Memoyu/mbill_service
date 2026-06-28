using Memo.Bill.Domain.Events.Permissions;

namespace Memo.Bill.Application.Permissions.Handlers;

public class CreatedPermissionEventHadler(
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : INotificationHandler<CreatedPermissionEvent>
{
    public async Task Handle(CreatedPermissionEvent notification, CancellationToken cancellationToken)
    {
        // 添加管理员关联权限
        await rolePermissionRepo.InsertAsync(new RolePermission
        {
            RoleId = InitConst.DefaultAdminId,
            PermissionId = notification.PermissionId,
        } ,cancellationToken);

        // TODO 后期添加管理平台时，需要去除
        // 添加用户关联权限
        await rolePermissionRepo.InsertAsync(new RolePermission
        {
            RoleId = InitConst.DefaultUserId,
            PermissionId = notification.PermissionId,
        }, cancellationToken);
    }
}
