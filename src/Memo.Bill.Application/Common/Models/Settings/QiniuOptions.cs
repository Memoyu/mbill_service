namespace Memo.Bill.Application.Common.Models.Settings;

public class QiniuOptions
{
    public const string Section = "Qiniu";

    public string AK { get; set; } = string.Empty;

    public string SK { get; set; } = string.Empty;

    public string Bucket { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public bool UseHttps { get; set; }
}
