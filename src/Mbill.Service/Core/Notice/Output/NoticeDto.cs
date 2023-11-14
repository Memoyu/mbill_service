namespace Mbill.Service.Core.Notice.Output;

public class NoticeDto : EntityDto
{
    public string Content { get; set; }

    public DateTime CreateTime { get; set; }
}
