namespace Mbill.Service.Core.Permission.Output;

public class RoleWithPermissionDto : EntityDto
{
    public List<RolePermissionDto> RolePermissions { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public bool IsStatic { get; set; }
    public int Sort { get; set; }
}