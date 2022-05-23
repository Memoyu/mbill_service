namespace mbill_service.Service.Core.Files;

public interface IFileSvc
{
    /// <summary>
    /// 单文件上传，键为file
    /// </summary>
    /// <param name="file">文件流</param>
    /// <param name="type">类型</param>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<FileDto> UploadAsync(IFormFile file, string type, int key = 0);
}
