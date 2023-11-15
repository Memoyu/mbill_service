namespace Mbill.Service.Bill.Asset.Output;

public class AssetDto
{
    public long BId { get; set; }

    public string Name { get; set; }

    public long ParentBId { get; set; }

    public string Type { get; set; }

    public decimal Amount { get; set; }

    public int Sort { get; set; }

    public string IconUrl { get; set; }

    public string Icon { get; set; }
}
