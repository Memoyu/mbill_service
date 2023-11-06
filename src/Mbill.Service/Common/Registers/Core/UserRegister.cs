using Mapster;

namespace Mbill.Service.Common.Registers.Core;

public class UserRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<UserEntity, UserDto>()
            .Map(d => d.Address, s => $"{s.Province}{s.City}{s.District}{s.Street}")
            .Map(d => d.GenderName, s => MapGender(s.Gender));

        config.ForType<UserEntity, LoginUserDto>()
             .Map(d => d.Gender, s => MapGender(s.Gender));
    }

    public string MapGender(int gender)
    {
        return gender == 0 ? "未知" : gender == 1 ? "男" : "女";
    }
}
