using Microsoft.Extensions.Options;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System.IO;

namespace Mbill.Service.Core.Files;

public class QiniuFileSvc : IQiniuFileSvc
{
    private readonly IFileRepo _fileRepo;
    private readonly QiniuClientOption _qiniuOption;

    public QiniuFileSvc(IFileRepo fileRepo, IOptionsMonitor<FileStorageOptions> option)
    {
        _fileRepo = fileRepo;
        _qiniuOption = option.CurrentValue?.Qiniu ?? throw new ArgumentNullException("没有七牛云相关配置");
    }

    public Task<ServiceResult<FileDto>> CheckMD5(string md5)
    {
        throw new NotImplementedException();
    }

    public ServiceResult<QiniuUploadTokenDto> GetUploadToken(string key = null)
    {
        var dto = new QiniuUploadTokenDto();
        dto.Token = CreateUploadToken(key);
        dto.Host = _qiniuOption.Host;
        return ServiceResult<QiniuUploadTokenDto>.Successed(dto);
    }

    public Task<ServiceResult<FileDto>> UploadAsync(IFormFile file, string type, int key = 0)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 获取上传Token
    /// </summary>
    /// <param name="key">文件路径</param>
    /// <returns></returns>
    public string CreateUploadToken(string key = null)
    {
        var sign = new Signature(new Mac(_qiniuOption.AK, _qiniuOption.SK));
        var policy = new PutPolicy
        {
            Scope = string.IsNullOrWhiteSpace(key) ? _qiniuOption.Bucket : $"{_qiniuOption.Bucket}:{key}"
        };
        return sign.SignWithData(policy.ToJsonString());

    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="path">文件上传路径 例如：xxxx/xxxxx/image.png</param>
    /// <param name="file">文件流</param>
    private (bool success, string msg, string domainKey) UploadStream(string path, Stream file)
    {

        return UploadStream(path, file, new Config { UseHttps = _qiniuOption.UseHttps }, new PutExtra { Version = "v2" });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">文件路径 例如：xxxx/xxxxx/image.png</param>
    /// <param name="file">文件流</param>
    /// <param name="config">请求上传配置信息</param>
    /// <param name="extra">请求上传拓展信息</param>
    /// <returns></returns>
    private (bool success, string msg, string domainKey) UploadStream(string path, Stream file, Config config, PutExtra extra)
    {
        FormUploader uploader = new FormUploader(config);
        HttpResult result = uploader.UploadStream(file, path, CreateUploadToken(), extra);
        var status = result.Code == (int)HttpCode.OK;
        return (status, result.Text, status ? $"{_qiniuOption.Host}{path}" : string.Empty);
    }
}
