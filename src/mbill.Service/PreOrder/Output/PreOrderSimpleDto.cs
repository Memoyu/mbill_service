namespace mbill.Service.PreOrder.Output;

public class PreOrderSimpleDto
{
    public long Id { get; set; }

    /// <summary>
    /// 分组Id
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// 预购金额
    /// </summary>
    public decimal PreAmount { get; set; }


    /// <summary>
    /// 实际金额
    /// </summary>
    public decimal RealAmount { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 记录日期：时间
    /// </summary>
    public string Time { get; set; }

    /// <summary>
    /// 状态 0:未购买；1：已购买
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    public string Color { get; set; }
}
