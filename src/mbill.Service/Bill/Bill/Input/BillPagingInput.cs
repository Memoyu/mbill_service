namespace mbill.Service.Bill.Bill.Input;

public class BillPagingInput : PagingDto
{
    /// <summary>
    /// 指定的日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 查询时间类型：0 查询月份，1 查询年份
    /// </summary>
    public int DateType { get; set; }

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
