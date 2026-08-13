namespace Memo.Bill.Application.Bills.Common;

public record BillQueryRequest : PaginationQuery
{
    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime? BeginDate { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 账单类型
    /// </summary>
    public BillType? Type { get; set; }

    /// <summary>
    /// 账单分类
    /// </summary>
    public List<long>? LedgerIds { get; set; }

    /// <summary>
    /// 账单分类
    /// </summary>
    public List<long>? CategoryIds { get; set; }

    /// <summary>
    /// 账单账户
    /// </summary>
    public List<long>? AccountIds { get; set; }

    /// <summary>
    /// 账单标签
    /// </summary>
    public List<long>? TagIds { get; set; }
}
