using Qiniu.Storage;
using System.IO;

namespace mbill.ToolKits.Qiniu;

public interface IQiniuClient
{
    /// <summary>
    /// 获取上传Token
    /// </summary>
    /// <param name="key">文件路径</param>
    /// <returns></returns>
    string CreateUploadToken(string key = null);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="path">文件上传路径 例如：xxxx/xxxxx/image.png</param>
    /// <param name="file">文件流</param>
    /// <returns></returns>
    (bool success, string msg, string domainKey) UploadStream(string path, Stream file);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">文件路径 例如：xxxx/xxxxx/image.png</param>
    /// <param name="file">文件流</param>
    /// <param name="config">请求上传配置信息</param>
    /// <param name="extra">请求上传拓展信息</param>
    /// <returns></returns>
    (bool success, string msg, string domainKey) UploadStream(string path, Stream file, Config config, PutExtra extra);
}