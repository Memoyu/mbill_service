namespace Mbill.Core.Common.Configs;

public class FileStorageOptions
{
    public int Default { get; set; }

    public LocalOption Local { get; set; }

    public QiniuClientOption Qiniu { get; set; }

}

public class LocalOption
{
    public string Host { get; set; }
}

public class QiniuClientOption
{
    public string AK { get; set; }

    public string SK { get; set; }

    public string Bucket { get; set; }

    public string Host { get; set; }

    public bool UseHttps { get; set; }
}