namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementTotalDto
    {
        //月统计
        public decimal MonthExpend { get; set; }
        public decimal MonthIcome { get; set; }
        public decimal MonthRepayment { get; set; }
        public decimal MonthTransfer { get; set; }

        //日统计
        public decimal DayExpend { get; set; }
        public decimal DayIcome { get; set; }
        public decimal DayRepayment { get; set; }
        public decimal DayTransfer { get; set; }
    }
}
