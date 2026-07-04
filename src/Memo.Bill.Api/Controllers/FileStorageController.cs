using Memo.Bill.Application.FileStorages.Command;

namespace Memo.Bill.Api.Controllers;

/// <summary>
/// 文件存储管理
/// </summary>
public class FileStorageController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// 生成七牛云上传Token
    /// </summary>
    /// <param name="request">存储路径</param>
    /// <returns></returns>
    [HttpGet("qiniu/upload/token")]
    public async Task<Result> CreateQiniuUploadTokenAsync([FromQuery] CreateQiniuUploadTokenCommand request)
    {
        return await mediator.Send(request);
    }
}
