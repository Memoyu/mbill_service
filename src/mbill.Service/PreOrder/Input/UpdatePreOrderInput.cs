namespace mbill.Service.PreOrder.Input;

public class UpdatePreOrderInput
{
    public long Id { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(40, ErrorMessage = "预购描述度不超过40")]
    public string Description { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    public DateTime Time { get; set; }
}
