using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单搜索记录表
/// </summary>
[Table(Name = "bill_search_record")]
public class BillSearchRecord : BaseAuditEntity
{
    /// <summary>
    /// 账单类型
    /// </summary>
    [Description("账单类型")]
    public BillType? Type { get; set; }

    /// <summary>
    /// 账单分类Id
    /// </summary>
    [Description("账单分类Id")]
    public long? CategoryId { get; set; }

    /// <summary>
    /// 账单账户Id
    /// </summary>
    [Description("账单账户Id")]
    public long? AccountId { get; set; }

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
