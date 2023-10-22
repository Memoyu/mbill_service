namespace Mbill.Service.Bill.Bill.Input;

public class MonthBillPagingInput : PagingDto
{
    /// <summary>
    /// 指定的年月
    /// </summary>
    public DateTime Month { get; set; }

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
