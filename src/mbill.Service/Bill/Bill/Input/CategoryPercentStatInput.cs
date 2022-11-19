namespace mbill.Service.Bill.Bill.Input;

public class CategoryPercentStatInput
{
    public DateTime Date { get; set; }

    /// <summary>
    /// 查询类型：
    /// 0 月统计
    /// 1 年统计
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 汇总类型：
    /// 0 根据父分类统计
    /// 1 根据子分类统计
    /// </summary>
    public int SummaryType { get; set; }

    /// <summary>
    /// 账单类型
    /// 0 支出
    /// 1 收入
    /// </summary>
    public int BillType { get; set; }

}
