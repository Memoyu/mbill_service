namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 上传文件表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_file")]
[Index("index_file_on_bid", "BId", false)]
public class FileEntity : FullAduitEntity
{
    /// <summary>
    /// 后缀
    /// </summary>
    [Column(StringLength = 50)]
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// 图片md5值，防止上传重复图片
    /// </summary>
    [Column(StringLength = 40)]
    public string Md5 { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 300)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 路径
    /// </summary>
    [Column(StringLength = 500)]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 大小
    /// </summary>
    public long? Size { get; set; }

    /// <summary>
    /// 1 local
    /// </summary>
    public short? Type { get; set; }

    public static string LocalFileService = "LocalFileService";
    public static Dictionary<string, string> SourceFileDic = new Dictionary<string, string>
        {
            { "asset","core/images/asset"},
            { "category","core/images/category"},
            { "avatar","core/images/avatar"},
            { "other","core/images/other"}
        };
}
