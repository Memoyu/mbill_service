namespace mbill.Service.Bill.Asset.Output;

public class AssetDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long ParentId { get; set; }

    public string Type { get; set; }

    public decimal Amount { get; set; }

    public int Sort { get; set; }

    public string IconUrl { get; set; }

    public string Icon { get; set; }
}
