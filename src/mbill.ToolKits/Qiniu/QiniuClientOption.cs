namespace mbill.ToolKits.Qiniu;

public class QiniuClientOption
{
    public string AK { get; set; }

    public string SK { get; set; }

    public string Bucket { get; set; }

    public string Host { get; set; }

    public bool UseHttps { get; set; }
}