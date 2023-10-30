namespace Mbill.Service.Bill.Bill.Output;

public class BillSimpleDto
{
    public long Id { get; set; }

    public string Category { get; set; }

    public string CategoryIcon { get; set; }

    public decimal Amount { get; set; }

    public string AmountFormat { get; set; }

    public int Type { get; set; }

    public string Description { get; set; }

    public string Date { get; set; }

    public string Time { get; set; }
}
