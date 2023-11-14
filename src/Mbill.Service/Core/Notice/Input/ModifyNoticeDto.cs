namespace Mbill.Service.Core.Notice.Input;

public class ModifyNoticeDto
{
    [Required(ErrorMessage ="公告内容必填")]
    [MaxLength(500, ErrorMessage ="公告内容长度不能超过500个字符")]
    public string Content { get; set; }

    /// <summary>
    /// 可见范围，用户BId 使用,分割
    /// </summary>
    public string Range { get; set; }
}
