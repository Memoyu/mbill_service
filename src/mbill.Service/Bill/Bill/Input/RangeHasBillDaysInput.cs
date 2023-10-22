namespace Mbill.Service.Bill.Bill.Input;

public class RangeHasBillDaysInput
{
    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndDate { get; set; }
}
