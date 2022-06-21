namespace mbill.Service.Bill.Bill.Output;

public class MonthTotalTrendStatDto : BaseChart<BaseSerie>
{
    public decimal ExpendHighest { get; set; }

    public decimal ExpendLowst { get; set; }

    public decimal IncomeHighest { get; set; }

    public decimal IncomeLowst { get; set; }
}
