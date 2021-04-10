using AutoMapper;
using Memoyu.Mbill.Application.Contracts.Dtos.Core;
using Memoyu.Mbill.Domain.Entities.Core;

namespace Memoyu.Mbill.Application.Contracts.Mapper.Core
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
