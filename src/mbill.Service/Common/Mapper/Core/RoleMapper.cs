namespace mbill.Service.Common.Mapper.Core;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<RoleEntity, RoleDto>();
        CreateMap<RoleEntity, RolePermissionDto>();
        CreateMap<ModifyRoleDto, RoleEntity>();
    }
}
