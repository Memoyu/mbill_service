namespace Memo.Bill.Application.Categories.Common;

internal record CategoryGroupResult : CategoryBaseResult
{
    public List<CategoryResult> Childs { get; set; } = [];
}
