namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 上传文件表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_file")]
public class FileEntity : FullAduitEntity
{
    /// <summary>
    /// 后缀
    /// </summary>
    [Description("后缀")]
    [Column(StringLength = 50)]
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// 图片md5值，防止上传重复图片
    /// </summary>
    [Description("图片md5值，防止上传重复图片")]
    [Column(StringLength = 40)]
    public string Md5 { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    [Description("名称")]
    [Column(StringLength = 300)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 路径
    /// </summary>
    [Description("路径")]
    [Column(StringLength = 500)]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 大小
    /// </summary>
    [Description("大小")]
    public long? Size { get; set; }

    /// <summary>
    /// 1 七牛, 99 自定义
    /// </summary>
    [Description("1 七牛, 99 自定义")]
    public short? Type { get; set; }
}
