using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Bills.Common;

internal record BillResult
{
    /// <summary>
    /// 账单Id
    /// </summary>
    public long BillId { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public CategoryBaseResult Category { get; set; } = new();

    /// <summary>
    /// 账户Id
    /// </summary>
    public AccountBaseResult Account { get; set; } = new();

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 类型：0-支出、1-收入
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 坐标
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 地点
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
