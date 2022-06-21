namespace mbill.Service.Bill.Bill.Input;

public class MonthTotalStatInput
{
    /// <summary>
    /// 指定的年月
    /// </summary>
    public DateTime Month { get; set; }

    /// <summary>
    /// 操作：
    /// 0 查询基本数据（支出、收入）
    /// 1 0的前提下加平均支出
    /// </summary>
    public int Opearte { get; set; }
}
