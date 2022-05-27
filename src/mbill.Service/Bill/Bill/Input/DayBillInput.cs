namespace mbill.Service.Bill.Bill.Input;

public class DayBillInput
{
    /// <summary>
    /// 指定的日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 账单类型
    /// </summary>
    public int? Type { get; set; }

    /// <summary>
    /// 账单分类
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 账单账户
    /// </summary>
    public long? AssetId { get; set; }

}
