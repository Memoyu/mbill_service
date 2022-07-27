using mbill.Service.Common.Converter;

namespace mbill.Service.Common.Mapper.Bill;

public class AssetMapper : Profile
{
    public AssetMapper()
    {
        CreateMap<CreateAssetInput, AssetEntity>();
        CreateMap<EditAssetInput, AssetEntity>();
        CreateMap<AssetEntity, AssetDto>()
            .ForMember(d => d.IconUrl, opt => opt.ConvertUsing<StringUrlConverter, string>(c => c.Icon));
        CreateMap<AssetEntity, AssetPageDto>();
    }
}
