﻿namespace Mbill.Service.Core.Permission.Input;

public class ModifyRoleDto
{
    public List<long> PermissionIds { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public bool IsStatic { get; set; }
    public int Sort { get; set; }
}