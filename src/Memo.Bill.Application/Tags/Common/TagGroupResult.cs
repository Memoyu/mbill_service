namespace Memo.Bill.Application.Tags.Common;

internal record TagGroupResult : TagBaseResult
{
    public List<TagResult> Childs { get; set; } = [];
}
