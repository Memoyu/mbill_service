namespace mbill.Service.Common.Mapper.Core;
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<ModifyUserDto, UserEntity>();
        CreateMap<ModifyUserBaseDto, UserEntity>();
        CreateMap<UserEntity, UserDto>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => $"{s.Province}{s.City}{s.District}{s.Street}"))
            .ForMember(d => d.GenderName, opt => opt.MapFrom(s => s.Gender == 0 ? "未知" : s.Gender == 1 ? "男" : "女"));
        CreateMap<UserEntity, PreLoginUserDto>();
        CreateMap<UserEntity, LoginUserDto>()
            .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Gender == 0 ? "未知" : s.Gender == 1 ? "男":"女"));
    }
}