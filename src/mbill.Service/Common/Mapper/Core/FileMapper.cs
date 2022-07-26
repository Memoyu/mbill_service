namespace mbill.Service.Common.Mapper.Core;

public class FileMapper : Profile
{
    public FileMapper(IFileRepo fileRepo)
    {
        CreateMap<MediaImageEntity, MediaImageDto>()
            .ForMember(d => d.Path, opt => opt.MapFrom(s => s.File == null ? "" : s.File.Path))
            .ForMember(d => d.Url, opt => opt.MapFrom(s => s.File == null ? "" : fileRepo.GetFileUrl(s.File.Path)));
    }
}