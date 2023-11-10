using Mapster;

namespace Mbill.Service.Common.Registers.Bill;

public class AssetRegister : BaseRegister
{
    protected override void TypeRegister(TypeAdapterConfig config)
    {
        config.ForType<AssetEntity, AssetDto>()
            .Map(d => d.IconUrl, s => UrlConverter(s.Icon));

        config.ForType<AssetEntity, AssetPageDto>()
             .Map(d => d.ParentName, s => s.Parent == null ? string.Empty : s.Parent.Name)
             .Map(d => d.TypeName, s => CategoryTypeConverter(s.Type))
             .Map(d => d.IconUrl, s => UrlConverter(s.Icon));
    }
}
