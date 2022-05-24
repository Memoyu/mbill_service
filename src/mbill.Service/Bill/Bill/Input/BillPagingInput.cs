namespace mbill.Service.Bill.Bill.Input;

public class BillPagingInput : PagingDto
{
    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? Day { get; set; }

    public string Type { get; set; }

    public long? CategoryId { get; set; }

    public long? AssetId { get; set; }

}
