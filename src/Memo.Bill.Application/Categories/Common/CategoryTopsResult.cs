namespace Memo.Bill.Application.Categories.Common;

internal record CategoryTopsResult
{
    public List<CategoryResult> Expends { get; set; } = [];

    public List<CategoryResult> Incomes { get; set; } = [];
}
