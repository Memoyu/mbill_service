using System.Text.Json.Serialization;

namespace Memo.Bill.Application.FileStorages.Common;

public class QiniuListFileResp
{
    public string Error { get; set; }

    [JsonPropertyName("error_code")]
    public string ErrorCode { get; set; }

    /// <summary>
    /// 返回条目的数组。不能用来判断是否还有剩余条目
    /// </summary>
    public List<QiniuListFileItem> Items { get; set; } = [];

    /// <summary>
    /// 起始条目标记，将作为下一次列举的参数传入。如果没有剩余条目则返回空字符串。
    /// </summary>
    public string Marker { get; set; }
}

public class QiniuListFileItem
{
    public string Key { get; set; }

    public string Hash { get; set; }

    public int Fsize { get; set; }

    public string MimeType { get; set; }

    public long PutTime { get; set; }

    public long LastModify { get; set; }

    public int Type { get; set; }

    public int Status { get; set; }

    public string Md5 { get; set; }
}
