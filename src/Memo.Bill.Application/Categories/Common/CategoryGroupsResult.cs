namespace Memo.Bill.Application.Categories.Common;

internal record CategoryGroupsResult
{
    public List<CategoryGroupItem> Expends { get; set; } = [];

    public List<CategoryGroupItem> Incomes { get; set; } = [];
}


internal record CategoryGroupItem : CategoryBaseResult
{
    public List<CategoryResult> Childs { get; set; } = [];
}
