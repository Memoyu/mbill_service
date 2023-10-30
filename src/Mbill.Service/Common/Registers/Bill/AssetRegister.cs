using Mapster;

namespace Mbill.Service.Common.Registers.Bill;

public class AssetRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<AssetEntity, AssetDto>()
       .Map(d => d.IconUrl, s => StringUrlConverter(s.Icon));
    }

    public string StringUrlConverter(string url)
    {
        var fileRepo = MapContext.Current.GetService<IFileRepo>();
        if (url.IsNullOrWhiteSpace()) return "";
        return fileRepo.GetFileUrl(url);
    }
}
