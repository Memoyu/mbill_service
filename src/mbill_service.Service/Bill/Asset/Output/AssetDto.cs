namespace mbill_service.Service.Bill.Asset.Output;

public class AssetDto : FullEntityDto
{

    public string Name { get; set; }

    public long ParentId { get; set; }

    public string Type { get; set; }

    public decimal Amount { get; set; }

    public int Sort { get; set; }

    public string IconUrl { get; set; }
}
