using mbill.Service.Common.Converter;

namespace mbill.Service.Common.Mapper.Core;

public class FileMapper : Profile
{
    public FileMapper()
    {
        CreateMap<MediaImageEntity, MediaImageDto>()
            .ForMember(d => d.Path, opt => opt.MapFrom(s => s.File == null ? "" : s.File.Path))
             .ForMember(d => d.Url, opt => opt.ConvertUsing<FileUrlConverter, FileEntity>(c => c.File));
    }
}