using mbill.Service.Core.Files.Input;

namespace mbill.Controllers.Core;

[Route("api/file")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
[Authorize]
public class FileController : ApiControllerBase
{
    private readonly IQiniuFileSvc _qiniuFileSvc;

    private readonly IMediaImageSvc _mediaImageSvc;

    public FileController(IQiniuFileSvc qiniuFileSvc, IMediaImageSvc mediaImageSvc)
    {
        _qiniuFileSvc = qiniuFileSvc;
        _mediaImageSvc = mediaImageSvc;
    }

    /// <summary>
    /// 获取上传文件token
    /// </summary>
    /// <param name="key">文件路径</param>
    /// <returns></returns>
    [HttpGet("upload-token")]
    [AllowAnonymous]
    //[LocalAuthorize("获取上传token", "文件管理")]
    public ServiceResult<QiniuUploadTokenDto> GetUploadToken([FromQuery] string key)
    {
        return _qiniuFileSvc.GetUploadToken(key);
    }

    /// <summary>
    /// 单文件上传，键为file
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [LocalAuthorize("上传单个文件", "文件管理")]
    public async Task<ServiceResult<FileDto>> UploadFile(IFormFile file, string type, int key = 0)
    {
        if (!MultipartRequestUtil.IsMultipartContentType(Request.ContentType))
        {
            throw new KnownException($"The request couldn't be processed (Error 1).");
        }
        return await _qiniuFileSvc.UploadAsync(file, type, key);
    }

    /// <summary>
    /// 获取账单分类分页
    /// </summary>
    /// <param name="pagingDto">分页参数</param>
    [HttpGet("media-image/pages")]
    [LocalAuthorize("获取分页媒体图片", "文件管理")]
    public async Task<ServiceResult<PagedDto<MediaImageDto>>> GetPageAsync([FromQuery] MediaImagePagingInput pagingDto)
    {
        return await _mediaImageSvc.GetPageAsync(pagingDto);
    }
}