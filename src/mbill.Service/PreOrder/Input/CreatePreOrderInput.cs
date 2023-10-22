namespace Mbill.Service.PreOrder.Input;
public class CreatePreOrderInput
{
    /// <summary>
    /// 分组Id
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal PreAmount { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(40, ErrorMessage = "预购描述度不超过40")]
    public string Description { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    public string Color { get; set; }
}
