using Mapster;

namespace Mbill.Service.Common.Registers.Core;

public class UserRegister : BaseRegister
{
    protected override void TypeRegister(TypeAdapterConfig config)
    {
        config.ForType<UserEntity, UserDto>()
            .Map(d => d.AvatarUrl, s => UrlConverter(s.AvatarUrl))
            .Map(d => d.Address, s => $"{s.Province}{s.City}{s.District}{s.Street}")
            .Map(d => d.GenderName, s => GenderConverter(s.Gender));

        config.ForType<UserEntity, UserWithRolesDto>()
            .Map(d => d.AvatarUrl, s => UrlConverter(s.AvatarUrl))
            .Map(d => d.Roles, s => s.UserRoles.Select(u => u.Role.Adapt<RoleDto>()))
            .Map(d => d.Address, s => $"{s.Province}{s.City}{s.District}{s.Street}")
            .Map(d => d.GenderName, s => GenderConverter(s.Gender));

        config.ForType<UserEntity, LoginUserDto>()
             .Map(d => d.Gender, s => GenderConverter(s.Gender));
    }
}
