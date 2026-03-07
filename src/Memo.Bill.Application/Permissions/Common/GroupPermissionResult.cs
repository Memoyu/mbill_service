namespace Memo.Bill.Application.Permissions.Common;

public class GroupPermissionResult
{
    public string Module { get; set; } = string.Empty;

    public string ModuleName { get; set; } = string.Empty;

    public List<PermissionResult> Permissions { get; set; } = [];
}
