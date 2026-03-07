namespace Memo.Bill.Domain.Constants.Security.Permissions;
public static partial class Permissions
{
    [Description("文件存储")]
    public static class FileStorage
    {
        [Description("生成七牛上传Token")]
        public const string GenerateQiniuUploadToken = "generate:token:qiniu:filestorage";
    }
}
