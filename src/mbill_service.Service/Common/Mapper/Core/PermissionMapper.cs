namespace mbill_service.Service.Common.Mapper.Core;

public class PermissionMapper : Profile
{
    public PermissionMapper()
    {
        CreateMap<PermissionEntity, PermissionDto>();
        CreateMap<ModifyPermissionDto, PermissionEntity>();
    }
}

