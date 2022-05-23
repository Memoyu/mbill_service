namespace mbill_service.Service.Core.Permission.Input;
public class PermissionDto
{
    public PermissionDto(string name, string module, string router)
    {
        Name = name;
        Module = module;
        Router = router;
    }

    public string Name { get; set; }
    public string Module { get; set; }
    public string Router { get; set; }
}
