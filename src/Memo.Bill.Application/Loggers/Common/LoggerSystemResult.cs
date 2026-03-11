using Serilog.Events;

namespace Memo.Bill.Application.Loggers.Common;

public class LoggerSystemResult
{
    public string Id { get; set; } = string.Empty;

    public LogEventLevel Level { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public string Request { get; set; } = string.Empty;

    public string ActionId { get; set; } = string.Empty;

    public string ActionName { get; set; } = string.Empty;

    public string RequestId { get; set; } = string.Empty;

    public string RequestPath { get; set; } = string.Empty;

    public string ExSource { get; set; } = string.Empty;

    public string ExMessage { get; set; } = string.Empty;

    public string ExStackTrace { get; set; } = string.Empty;

    public DateTime Time { get; set; }
}
