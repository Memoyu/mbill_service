namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementTotalInputDto
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public long? UserId { get; set; }
    }
}
