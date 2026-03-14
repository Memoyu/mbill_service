using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Bills.Common;

internal record BillSummaryCategoryItem
{
    /// <summary>
    /// 分类
    /// </summary>
    public CategoryBaseResult Category { get; set; } = new();

    /// <summary>
    /// 总金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 子分类
    /// </summary>
    public List<BillSummaryCategoryItem> Childs { get; set; } = [];
}
