namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单搜索记录表
/// </summary>
[Table(Name = "bill_search_record")]
public class BillSearchRecord : BaseAuditEntity
{
    /// <summary>
    /// 账单类型 JSON
    /// </summary>
    [Description("账单类型 JSON")]
    [Column(StringLength = 100)]
    public string? Types { get; set; }

    /// <summary>
    /// 账单分类Id JSON
    /// </summary>
    [Description("账单分类Id JSON")]
    [Column(StringLength = 500)]
    public string? CategoryIds { get; set; }

    /// <summary>
    /// 账单账户Id JSON
    /// </summary>
    [Description("账单账户Id JSON")]
    [Column(StringLength = 500)]
    public string? AccountIds { get; set; }

    /// <summary>
    /// 金额区间最小值
    /// </summary>
    [Description("金额区间最小值")]
    [Column(Precision = 12, Scale = 2)]
    public decimal? AmountMin { get; set; }

    /// <summary>
    /// 金额区间最大值
    /// </summary>
    [Description("金额区间最大值")]
    [Column(Precision = 12, Scale = 2)]
    public decimal? AmountMax { get; set; }

    /// <summary>
    /// 账单时间起始
    /// </summary>
    [Description("账单时间起始")]
    public DateTime? DateBegin { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    [Description("账单时间截止")]
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    [Description("搜索关键字")]
    public string? KeyWord { get; set; }
}
