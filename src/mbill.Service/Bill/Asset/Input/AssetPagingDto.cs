namespace mbill.Service.Bill.Asset.Input;

public class AssetPagingDto : PagingDto
{
    public string AssetName { get; set; }

    public string ParentIds { get; set; }

    public string Type { get; set; }

    public DateTime? CreateStartTime { get; set; }

    public DateTime? CreateEndTime { get; set; }
}
