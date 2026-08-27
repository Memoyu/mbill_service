using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Domain.Entities;

/// <summary>
/// 频次记录表
/// 账单分类、账单账户、账单标签等
/// </summary>
[Table(Name = "frequency_record")]
[Index("idx_frequency_record_record_id", nameof(RecordId), false)]
public class FrequencyRecord : BaseEntity
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
    public FrequencyRecordType Type { get; set; }

    /// <summary>
    /// 频次
    /// </summary>
    [Description("频次")]
    public long Frequency { get; set; }
}
