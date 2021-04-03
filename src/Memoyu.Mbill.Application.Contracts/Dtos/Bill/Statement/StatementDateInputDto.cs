namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementDateInputDto
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string Type { get; set; }
        public long? UserId { get; set; }
    }
}
