namespace mbill.Service.Bill.Bill.Output;

public class BillSearchRecordOutput
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
