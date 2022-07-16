namespace mbill.Controllers.Core;

[Route("api/file")]
[ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
[Authorize]
public class FileController : ApiControllerBase
{
    private readonly IQiniuFileSvc _qiniuFileSvc;
    public FileController(IQiniuFileSvc qiniuFileSvc)
    {
        _qiniuFileSvc = qiniuFileSvc;
    }

    /// <summary>
    /// 获取上传文件token
    /// </summary>
    /// <returns></returns>
    [HttpGet("upload-token")]
    [LocalAuthorize("获取上传token", "文件管理")]
    public ServiceResult<string> GetUploadToken()
    {
        return _qiniuFileSvc.GetUploadToken();
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

}