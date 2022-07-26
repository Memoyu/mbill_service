namespace mbill.Service.Common.Mapper.Bill;

public class AssetMapper : Profile
{
    public AssetMapper()
    {
        CreateMap<CreateAssetInput, AssetEntity>();
        CreateMap<EditAssetInput, AssetEntity>();
        CreateMap<AssetEntity, AssetDto>();
        CreateMap<AssetEntity, AssetPageDto>();
    }
}
