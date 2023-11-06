namespace Mbill.Service.PreOrder.Input;

public class UpdatePreOrderStatusInput
{
    public long BId { get; set; }

    /// <summary>
    /// 账单Id
    /// </summary>
    public long RealAmount { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }
}
