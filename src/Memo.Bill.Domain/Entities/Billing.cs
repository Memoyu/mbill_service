namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单表
/// </summary>
[Table(Name = "billing")]
[Index("idx_billing_bill_id", nameof(BillId), false)]
[Index("idx_billing_category_id", nameof(CategoryId), false)]
[Index("idx_billing_asset_id", nameof(AssetId), false)]
public class Billing : BaseAuditEntity
{
    /// <summary>
    /// 账单Id
    /// </summary>
    [Snowflake]
    [Description("账单Id")]
    [Column(CanUpdate = false)]
    public long BillId { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    [Description("分类Id")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 资产Id
    /// </summary>
    [Description("资产Id")]
    public long AssetId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [Description("金额")]
    [Column(Precision = 12, Scale = 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// 类型：0-支出、1-收入
    /// </summary>
    [Description("类型：0-支出、1-收入")]
    public int Type { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Description("备注")]
    [Column(StringLength = 200, IsNullable = false)]
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 坐标
    /// </summary>
    [Description("坐标")]
    [Column(StringLength = 100)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 地点
    /// </summary>
    [Description("地点")]
    [Column(StringLength = 200)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// 日期
    /// </summary>
    [Description("日期")]
    public DateTime Date { get; set; }
}
