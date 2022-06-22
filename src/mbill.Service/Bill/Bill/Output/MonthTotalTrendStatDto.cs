namespace mbill.Service.Bill.Bill.Output;

public class MonthTotalTrendStatDto : BaseChart<BaseSerie>
{
    public string ExpendHighest { get; set; }

    public string ExpendLowst { get; set; }

    public string IncomeHighest { get; set; }

    public string IncomeLowst { get; set; }
}
