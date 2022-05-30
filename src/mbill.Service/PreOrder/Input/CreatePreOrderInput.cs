namespace mbill.Service.PreOrder.Input;
public class CreatePreOrderInput
{
    /// <summary>
    /// 分类Id
    /// </summary>
    [Required(ErrorMessage = "必须传入分类Id")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(200, ErrorMessage = "预购描述度不超过200")]
    public string Description { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    public DateTime Time { get; set; }
}
