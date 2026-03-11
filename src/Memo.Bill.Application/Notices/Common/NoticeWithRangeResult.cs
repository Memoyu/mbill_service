namespace Memo.Bill.Application.Notices.Common;

public record NoticeWithRangeResult : NoticeResult
{
    /// <summary>
    /// 可见范围
    /// </summary>
    public string Range { get; set; } = "[]";
}
