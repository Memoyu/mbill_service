namespace mbill.Service.Bill.Bill.Output;

public class BillTotalDto
{
    //月统计
    public decimal MonthExpend { get; set; }
    public decimal MonthIncome { get; set; }
    public decimal MonthRepayment { get; set; }
    public decimal MonthTransfer { get; set; }

    //日统计
    public decimal DayExpend { get; set; }
    public decimal DayIncome { get; set; }
    public decimal DayRepayment { get; set; }
    public decimal DayTransfer { get; set; }
}