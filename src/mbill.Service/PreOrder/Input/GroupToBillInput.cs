namespace mbill.Service.PreOrder.Input;

public class GroupToBillInput
{
    public long Id { get; set; }

    /// <summary>
    /// 账单Id
    /// </summary>
    public long BillId { get; set; }
}
