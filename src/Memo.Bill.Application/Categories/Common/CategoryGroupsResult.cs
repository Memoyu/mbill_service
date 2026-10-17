namespace Memo.Bill.Application.Categories.Common;

internal record CategoryGroupsResult
{
    public CategoryGroupItem Expends { get; set; } = new();

    public CategoryGroupItem Incomes { get; set; } = new();
}

internal record CategoryGroupItem
{
    public List<CategoryWithChildsResult> Tops { get; set; } = [];

    public List<CategoryWithChildsResult> List { get; set; } = [];
}


