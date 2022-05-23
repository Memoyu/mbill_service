namespace mbill_service.Service.Bill.Statement.Output;

public class BillDateInputDto
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string Type { get; set; }
    public long? UserId { get; set; }
}
