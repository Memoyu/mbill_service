namespace Memo.Bill.Application.Bills.Common;

internal record BillSearchRecordResult
{
    /// <summary>
    /// 账单类型
    /// </summary>
    public BillType? Type { get; set; }

    /// <summary>
    /// 账单分类Id
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 账单账户Id
    /// </summary>
    public long? AccountId { get; set; }

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
    public DateTime? DateBegin { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string? KeyWord { get; set; }
}
