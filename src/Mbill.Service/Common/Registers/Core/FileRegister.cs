using Mapster;

namespace Mbill.Service.Common.Registers.Core;

public class FileRegister : BaseRegister
{
    protected override void TypeRegister(TypeAdapterConfig config)
    {
        config.ForType<MediaImageEntity, MediaImageDto>()
             .Map(d => d.Path, s => s.File == null ? "" : s.File.Path)
             .Map(d => d.Url, s => UrlConverter(s.File == null ? string.Empty : s.File.Path));
    }
}
