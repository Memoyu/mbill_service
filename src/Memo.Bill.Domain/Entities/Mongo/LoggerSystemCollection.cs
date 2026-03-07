using Memo.Bill.Domain.Constants;
using Serilog.Sinks.MongoDB;

namespace Memo.Bill.Domain.Entities.Mongo;

/// <summary>
/// 系统日志
/// </summary>
[MongoCollection(AppConst.LoggerSystemCollectionName)]
public class LoggerSystemCollection : LogEntry
{
}
