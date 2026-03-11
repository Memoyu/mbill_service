using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Application.Roles.Common;

public class RoleListResult
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 角色唯一标识字符
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色类型
    /// </summary>
    public RoleType Type { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
