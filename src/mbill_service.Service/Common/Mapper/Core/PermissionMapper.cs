using AutoMapper;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Permission.Input;

namespace mbill_service.Service.Common.Mapper.Core
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            CreateMap<PermissionEntity, PermissionDto>();
            CreateMap<ModifyPermissionDto, PermissionEntity>();
        }
    }
}
