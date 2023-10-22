namespace Mbill.Service.Bill.Bill.Output;

public class BillDto
{
    public long Id { get; set; }

    public long CategoryId { get; set; }

    public long AssetId { get; set; }

    public decimal Amount { get; set; }

    public int Type { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }

    public DateTime Time { get; set; }
}
