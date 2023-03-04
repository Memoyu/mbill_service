namespace mbill.Service.Bill.Bill.Input;

public class BillSearchPagingInput : PagingDto
{
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

    /// <summary>
    /// 金额区间最小值
    /// </summary>
    public decimal? AmountMin { get; set; }

    /// <summary>
    /// 金额区间最大值
    /// </summary>
    public decimal? AmountMax { get; set; }

    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime? TimeBegin { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime? TimeEnd { get; set; }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string KeyWord { get; set; }
}
