namespace Mbill.Service.Bill.Asset.Output;

public class AssetGroupDto
{
    public long BId { get; set; }

    public string Name { get; set; }

    public List<AssetDto> Childs { get; set; }
}
