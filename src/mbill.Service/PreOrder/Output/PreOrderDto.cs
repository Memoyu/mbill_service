namespace mbill.Service.PreOrder.Output;

public class PreOrderDto : EntityDto
{
    /// <summary>
    /// 分组Id
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    public string TimeFormat { get; set; }
 

    /// <summary>
    /// 状态 0:未购买；1：已购买
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    public string Color { get; set; }
}
