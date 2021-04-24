using AutoMapper;
using mbill_service.Core.Domains.Entities.Core;
using mbill_service.Service.Core.Permission.Input;
using mbill_service.Service.Core.Permission.Output;

namespace mbill_service.Service.Common.Mapper.Core
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<RoleEntity, RolePermissionDto>();
            CreateMap<ModifyRoleDto, RoleEntity>();
        }
    }

}
