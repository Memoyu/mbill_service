namespace mbill_service.Service.Core.Permission.Output;

public class RoleDto : EntityDto
{
    public string Name { get; set; }
    public string Info { get; set; }
    public bool IsStatic { get; set; }
    public int Sort { get; set; }
}