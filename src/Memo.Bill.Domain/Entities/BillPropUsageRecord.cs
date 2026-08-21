using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 账单项使用记录表
/// 账单分类、账单账户、账单标签等
/// </summary>
[Table(Name = "billing")]
[Index("idx_bill_prop_usage_record_record_id", nameof(RecordId), false)]
public class BillPropUsageRecord : BaseEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [Snowflake]
    [Column(CanUpdate = false)]
    [Description("记录Id")]
    public long RecordId { get; set; }

    /// <summary>
    /// 记录类型
    /// </summary>
    [Description("记录类型")]
    public BillPropUsageRecordType RecordType { get; set; }

    /// <summary>
    /// 类型：0 支出，1 收入
    /// </summary>
    [Description("类型：0-支出，1-收入")]
    public BillType? Type { get; set; }

    /// <summary>
    /// 记录次数
    /// </summary>
    [Description("记录次数")]
    public long Frequency { get; set; }
}
