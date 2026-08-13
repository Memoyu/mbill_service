namespace Memo.Bill.Application.Bills.Common;

public record BillSearchRequest : BillQueryRequest
{
    /// <summary>
    /// 关键字
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// 金额最小值
    /// </summary>
    public decimal? AmountMin { get; set; }

    /// <summary>
    /// 金额 最大值
    /// </summary>
    public decimal? AmountMax { get; set; }

    /// <summary>
    /// 排除的账单
    /// </summary>
    public List<long>? ExcludeBillIds { get; set; }
}
