namespace Memo.Bill.Application.Categories.Common;

internal record CategoryGroupResult
{
    public long CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<CategoryResult> Childs { get; set; } = [];
}
