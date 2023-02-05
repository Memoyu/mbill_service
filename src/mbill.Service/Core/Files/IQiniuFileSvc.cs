namespace mbill.Service.Core.Files;

public interface IQiniuFileSvc
{
    /// <summary>
    /// 校验MD5文件是否存在
    /// </summary>
    /// <param name="md5"></param>
    /// <returns></returns>
    Task<ServiceResult<FileDto>> CheckMD5(string md5);

    /// <summary>
    /// 获取上传Token
    /// </summary>
    /// <param name="key">文件路径</param>
    /// <returns></returns>
    ServiceResult<QiniuUploadTokenDto> GetUploadToken(string key = null);

    /// <summary>
    /// 单文件上传，键为file
    /// </summary>
    /// <param name="file">文件流</param>
    /// <param name="type">类型</param>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<ServiceResult<FileDto>> UploadAsync(IFormFile file, string type, int key = 0);
}
