namespace Memo.Bill.Application.Notices.Common;

public record NoticeResult
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
