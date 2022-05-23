namespace mbill.Service.Common.Mapper.Bill;

public class AssetMapper : Profile
{
    public AssetMapper()
    {
        CreateMap<ModifyAssetDto, AssetEntity>();
        CreateMap<AssetEntity, AssetDto>();
        CreateMap<AssetEntity, AssetPageDto>();
    }
}
