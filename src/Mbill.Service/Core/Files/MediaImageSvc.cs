using MapsterMapper;

namespace Mbill.Service.Core.Files;

public class MediaImageSvc : IMediaImageSvc
{
    private readonly IMapper _mapper;

    private readonly IMediaImageRepo _mediaImageRepo;

    public MediaImageSvc(IMediaImageRepo mediaImageRepo, IMapper mapper)
    {
        _mapper = mapper;
        _mediaImageRepo = mediaImageRepo;
    }

    public async Task<ServiceResult<PagedDto<MediaImageDto>>> GetPageAsync(MediaImagePagingInput pagingDto)
    {
        var list = await _mediaImageRepo
           .Select
           .Include(r => r.File)
           .Where(r => r.Type == pagingDto.Type)
           .ToPageListAsync(pagingDto, out long totalCount);

        var dtos = list.Select(l => _mapper.Map<MediaImageDto>(l)).ToList();
        return ServiceResult<PagedDto<MediaImageDto>>.Successed(new PagedDto<MediaImageDto> (dtos, totalCount ));
    }
}
