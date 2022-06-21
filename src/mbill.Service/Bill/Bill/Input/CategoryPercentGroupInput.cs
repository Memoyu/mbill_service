namespace mbill.Service.Bill.Bill.Input;

public class CategoryPercentGroupInput
{
    public DateTime Date { get; set; }

    /// <summary>
    /// 查询类型：
    /// 0 月统计
    /// 1 年统计
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 账单类型
    /// 0 支出
    /// 1 收入
    /// </summary>
    public int BillType { get; set; }
}
