using Memo.Bill.Domain.Constants;

namespace Memo.Bill.Domain.Entities.Mongo;

/// <summary>
/// 用户日志
/// </summary>
[MongoCollection(AppConst.LoggerUserCollectionName)]
public class LoggerUserCollection
{
    /// <summary>
    /// 访问IP
    /// </summary>
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// 访问路径
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 访问时间
    /// </summary>
    public DateTime VisitTime { get; set; }
}
