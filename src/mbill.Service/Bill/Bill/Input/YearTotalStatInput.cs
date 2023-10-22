namespace Mbill.Service.Bill.Bill.Input;

public class YearTotalStatInput
{
    /// <summary>
    /// 指定的年
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 操作：
    /// 0 查询基本数据（支出、收入、预购金额）
    /// 1 0的前提下加平均支出
    /// </summary>
    public int Opearte { get; set; }
}
