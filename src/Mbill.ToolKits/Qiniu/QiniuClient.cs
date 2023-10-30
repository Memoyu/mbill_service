using Microsoft.Extensions.Options;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace Mbill.ToolKits.Qiniu;

public class QiniuClient : IQiniuClient
{
    private readonly QiniuClientOption _option;

    private readonly Signature _sign;


    public QiniuClient(IOptionsMonitor<QiniuClientOption> option)
    {
        _option = option.CurrentValue;
        _sign = new Signature(new Mac(_option.AK, _option.SK));
    }

    public string CreateUploadToken(string key = null)
    {
        var policy = new PutPolicy
        {
            Scope = string.IsNullOrWhiteSpace(key) ? _option.Bucket : $"{_option.Bucket}:{key}"
        };
        return _sign.SignWithData(policy.ToJsonString());

    }

    public (bool success, string msg, string domainKey) UploadStream(string path, Stream file)
    {

        return UploadStream(path, file, new Config { UseHttps = _option.UseHttps }, new PutExtra { Version = "v2" });
    }

    public (bool success, string msg, string domainKey) UploadStream(string path, Stream file, Config config, PutExtra extra)
    {
        FormUploader uploader = new FormUploader(config);
        HttpResult result = uploader.UploadStream(file, path, CreateUploadToken(), extra);
        var status = result.Code == (int)HttpCode.OK;
        return (status, result.Text, status ? $"{_option.Host}{path}" : string.Empty);
    }
}