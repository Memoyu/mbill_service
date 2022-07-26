namespace mbill.Service.Core.Files;

public interface IMediaImageSvc
{
    /// <summary>
    /// 校验MD5文件是否存在
    /// </summary>
    /// <param name="pagingDto">媒体图片分页参数</param>
    /// <returns></returns>
    Task<ServiceResult<PagedDto<MediaImageDto>>> GetPageAsync(MediaImagePagingInput pagingDto);
}
