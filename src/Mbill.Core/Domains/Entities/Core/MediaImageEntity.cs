namespace Mbill.Core.Domains.Entities.Core;

/// <summary>
/// 图片媒体文件信息表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_media_image")]
[Index("index_media_image_on_file_bid", nameof(FileBId), false)]
[Index("index_media_image_on_type", nameof(Type), false)]
public class MediaImageEntity : FullAduitEntity
{
    /// <summary>
    /// 文件BId
    /// </summary>
    [Description("文件BId")]
    public long FileBId { get; set; }

    /// <summary>
    /// 图片类型：0 Icon, 1 background
    /// </summary>
    [Description("图片类型：0 Icon, 1 background")]
    public int Type { get; set; }


    [Navigate(nameof(FileBId), TempPrimary = nameof(BId))]
    public virtual FileEntity File { get; set; }
}
