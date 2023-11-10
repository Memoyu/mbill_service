namespace Mbill.Service.Core.User.Output
{
    public class UserWithRolesDto : UserDto
    {
        public List<RoleDto> Roles { get; set; }
    }
}
