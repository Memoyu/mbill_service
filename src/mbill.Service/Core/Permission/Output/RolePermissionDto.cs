namespace mbill.Service.Core.Permission.Output;

public class RolePermissionDto : EntityDto
{
    public List<RolePermissionEntity> RolePermissions { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public bool IsStatic { get; set; }
    public int Sort { get; set; }
}