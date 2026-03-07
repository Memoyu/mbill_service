using Serilog.Events;
namespace Memo.Bill.Application.Logger.Queries.System.Page;

[Authorize(Permissions = ApiPermission.LoggerSystem.Page)]
public record PageLoggerSystemQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    public string? Id { get; set; }

    public LogEventLevel? Level { get; set; }

    public string? Message { get; set; }

    public string? Source { get; set; }

    public string? RequestParamterName { get; set; }

    public string? RequestParamterValue { get; set; }

    public string? RequestId { get; set; }

    public string? RequestPath { get; set; }

    public DateTime? DateBegin { get; set; }

    public DateTime? DateEnd { get; set; }
}
