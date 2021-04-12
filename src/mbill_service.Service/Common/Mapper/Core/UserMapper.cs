using AutoMapper;
using mbill_service.Core.Domains.Entities.User;
using mbill_service.Service.Common.Common.Converter;
using mbill_service.Service.Core.User.Input;
using mbill_service.Service.Core.User.Output;

namespace mbill_service.Service.Common.Mapper.Core
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ModifyUserDto, UserEntity>();
            CreateMap<UserEntity, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(s => $"{s.Province}{s.City}{s.District}{s.Street}"))
                .ForMember(d => d.Gender, opt => opt.ConvertUsing<GenderFormatter, int>());
        }
    }
}
