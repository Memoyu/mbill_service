namespace mbill.Service.Bill.Bill.Output;

public class BillDateWithTotalDto
{
    /// <summary>
    /// 年
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 月
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// 日
    /// </summary>
    public int Day { get; set; }

    /// <summary>
    /// 账单总数
    /// </summary>
    public int Total { get; set; }
}
