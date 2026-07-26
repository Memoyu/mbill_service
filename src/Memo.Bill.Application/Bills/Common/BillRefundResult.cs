
using Memo.Bill.Application.Accounts.Common;

namespace Memo.Bill.Application.Bills.Common;

internal record BillRefundResult
{
    /// <summary>
    /// 退款Id
    /// </summary>
    public long RefundId { get; set; }

    /// <summary>
    /// 账单Id
    /// </summary>
    public long BillId { get; set; }

    /// <summary>
    /// 账户Id
    /// </summary>
    public AccountBaseResult Account { get; set; } = new();

    /// <summary>
    /// 退款金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 账单金额
    /// </summary>
    public decimal AmountBefore { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime Date { get; set; }
}
