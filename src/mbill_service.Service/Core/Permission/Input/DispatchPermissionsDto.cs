namespace mbill_service.Service.Core.Permission.Input;

public class DispatchPermissionsDto
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 权限Id集合
    /// </summary>
    public List<long> PermissionIds { get; set; }
}

