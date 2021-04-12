using AutoMapper;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Permission.Input;

namespace mbill_service.Service.Common.Mapper.Core
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<ModifyRoleDto, RoleEntity>();
        }
    }

}
