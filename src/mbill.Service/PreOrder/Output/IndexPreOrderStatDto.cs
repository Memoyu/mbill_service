namespace mbill.Service.PreOrder.Output;

public class IndexPreOrderStatDto
{
    public long Total { get; set; }

    public decimal PreAmount { get; set; }

    public long Done { get; set; }

    public long UnDone { get; set; }
}
