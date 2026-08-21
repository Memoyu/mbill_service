namespace Memo.Bill.Application.Categories.Common;

internal record CategoryGroupResult
{
    /// <summary>
    /// 常用项
    /// 数量为10个
    /// </summary>
    public List<CategoryResult> ExpendTops { get; set; } = [];

    public List<CategoryResult> IncomeTops { get; set; } = [];

    public List<CategoryGroupItem> Expends { get; set; } = [];

    public List<CategoryGroupItem> Incomes { get; set; } = [];
}


internal record CategoryGroupItem : CategoryBaseResult
{
    public List<CategoryResult> Childs { get; set; } = [];
}
