namespace Mbill.Service.Bill.Bill.Output;

public class BillDto
{
    public long BId { get; set; }

    public long CategoryBId { get; set; }

    public long AssetBId { get; set; }

    public decimal Amount { get; set; }

    public int Type { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }

    public DateTime Time { get; set; }
}
