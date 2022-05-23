namespace mbill.Service.Core.Logger.Input;

public class LogPagingDto : PagingDto
{
    public string Method { get; set; }

    public string Username { get; set; }

    public string UserId { get; set; }

    public string StatusCode { get; set; }

    public DateTime? CreateStartTime { get; set; }

    public DateTime? CreateEndTime { get; set; }
}
