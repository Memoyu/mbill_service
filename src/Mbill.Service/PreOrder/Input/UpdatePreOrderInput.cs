namespace Mbill.Service.PreOrder.Input;

public class UpdatePreOrderInput : BaseUpdateInput
{
    /// <summary>
    /// 预购金额
    /// </summary>
    public decimal PreAmount { get; set; }

    /// <summary>
    /// 实购金额
    /// </summary>
    public decimal RealAmount { get; set; }

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
