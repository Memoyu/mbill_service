namespace mbill.Service.Core.User.Input;

public class UserPagingDto : PagingDto
{
    public string Username { get; set; }

    public string Nickname { get; set; }

    public int IsEnable { get; set; }

    public long RoleId { get; set; }

    public DateTime? CreateStartTime { get; set; }

    public DateTime? CreateEndTime { get; set; }
}