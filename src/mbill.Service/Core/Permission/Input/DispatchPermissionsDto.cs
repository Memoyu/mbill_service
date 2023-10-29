namespace Mbill.Service.Core.Permission.Input;

public class DispatchPermissionsDto
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleBId { get; set; }

    /// <summary>
    /// 权限Id集合
    /// </summary>
    public List<long> PermissionBIds { get; set; }
}

