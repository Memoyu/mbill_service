using Mbill.ToolKits.Qiniu;
using Microsoft.Extensions.Options;
using System.Text;

namespace Mbill.Service.Core.Files;

public class QiniuFileSvc : IQiniuFileSvc
{
    private readonly IFileRepo _fileRepo;
    private readonly QiniuClientOption _qiniuClientOption;
    private readonly IQiniuClient _qiniuClient;

    public QiniuFileSvc(IWebHostEnvironment hostingEnv, IFileRepo fileRepo, IQiniuClient qiniuClient, IOptionsMonitor<QiniuClientOption> option)
    {
        _fileRepo = fileRepo;
        _qiniuClient = qiniuClient;
        _qiniuClientOption = option.CurrentValue;
    }

    public Task<ServiceResult<FileDto>> CheckMD5(string md5)
    {
        throw new NotImplementedException();
    }

    public ServiceResult<QiniuUploadTokenDto> GetUploadToken(string key = null)
    {
        var dto = new QiniuUploadTokenDto();
        dto.Token = _qiniuClient.CreateUploadToken(key);
        dto.Host = _qiniuClientOption.Host;
        return ServiceResult<QiniuUploadTokenDto>.Successed(dto);
    }

    public Task<ServiceResult<FileDto>> UploadAsync(IFormFile file, string type, int key = 0)
    {
        throw new NotImplementedException();
    }
}
