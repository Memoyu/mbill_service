using Serilog.Events;

namespace Memo.Bill.Application.Loggers.Common;

public class LoggerSystemPageResult
{
    public string Id { get; set; } = string.Empty;

    public LogEventLevel Level { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public string Request { get; set; } = string.Empty;

    public string RequestId { get; set; } = string.Empty;

    public string RequestPath { get; set; } = string.Empty;

    public string ExMessage { get; set; } = string.Empty;

    public DateTime Time { get; set; }
}
