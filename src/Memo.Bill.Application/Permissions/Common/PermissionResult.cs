using Memo.Bill.Application.Roles.Common;

namespace Memo.Bill.Application.Permissions.Common;

public record PermissionResult
{
    /// <summary>
    /// 权限Id
    /// </summary>
    public long PermissionId { get; set; }

    /// <summary>
    /// 权限所属模块
    /// </summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// 权限所属模块
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 权限标识
    /// </summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// 关联角色数
    /// </summary>
    public List<RoleListResult> Roles { get; set; } = [];
}
