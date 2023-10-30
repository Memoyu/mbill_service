using Mapster;

namespace Mbill.Service.Common.Registers.Core;

public class FileRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<MediaImageEntity, MediaImageDto>()
             .Map(d => d.Path, s => s.File == null ? "" : s.File.Path)
             .Map(d => d.Url, s => FileUrlConvert(s.File));
    }

    public string FileUrlConvert(FileEntity file)
    {
        var fileRepo = MapContext.Current.GetService<IFileRepo>();
        if (file == null) return "";
        return fileRepo.GetFileUrl(file.Path);
    }

}
