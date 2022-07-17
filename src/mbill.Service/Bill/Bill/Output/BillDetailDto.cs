namespace mbill.Service.Bill.Bill.Output;

public class BillDetailDto : BillDto
{
    public string Asset { get; set; }

    public string Category { get; set; }

    public string CategoryIcon { get; set; }

    public string AssetIcon { get; set; }

    public string AmountFormat { get; set; }

    public string TimeFormat { get; set; }
}
