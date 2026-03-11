using Memo.Bill.Application.Loggers.Common;
using Memo.Bill.Domain.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Events;
namespace Memo.Bill.Application.Loggers.Queries.System;

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


public class PageLoggerSystemQueryHandler(
    IMapper mapper,
    IBaseMongoRepository<LoggerSystemCollection> systemLogMongoRepo
    ) : IRequestHandler<PageLoggerSystemQuery, Result>
{
    public async Task<Result> Handle(PageLoggerSystemQuery request, CancellationToken cancellationToken)
    {
        var f = Builders<LoggerSystemCollection>.Filter.Empty;
        if (!string.IsNullOrWhiteSpace(request.Id))
            f &= Builders<LoggerSystemCollection>.Filter.Eq(nameof(LoggerSystemCollection.Id), ObjectId.Parse(request.Id));

        if (request.Level.HasValue)
            f &= Builders<LoggerSystemCollection>.Filter.Eq(nameof(LoggerSystemCollection.Level), request.Level.Value);

        if (!string.IsNullOrWhiteSpace(request.Message))
            f &= Builders<LoggerSystemCollection>.Filter.Regex(u => u.RenderedMessage, new BsonRegularExpression(request.Message, "i"));

        if (!string.IsNullOrWhiteSpace(request.Source))
            f &= Builders<LoggerSystemCollection>.Filter.Regex("Properties.SourceContext", new BsonRegularExpression(request.Source, "i"));

        if (!string.IsNullOrWhiteSpace(request.RequestParamterName) && !string.IsNullOrWhiteSpace(request.RequestParamterValue))
        {
            f &= new BsonDocument("$expr",
                     new BsonDocument("$regexMatch",
                         new BsonDocument
                         {
                            { "input", new BsonDocument("$toString", "$Properties.Request." + request.RequestParamterName ) },
                            { "regex", request.RequestParamterValue },
                            { "options", "i" }
                         }
                     )
                 );
        }

        if (!string.IsNullOrWhiteSpace(request.RequestId))
            f &= Builders<LoggerSystemCollection>.Filter.Eq("Properties.RequestId", request.RequestId);

        if (!string.IsNullOrWhiteSpace(request.RequestPath))
            f &= Builders<LoggerSystemCollection>.Filter.Regex("Properties.RequestPath", new BsonRegularExpression(request.RequestPath, "i"));

        if (request.DateBegin.HasValue && request.DateEnd.HasValue)
        {
            f &= Builders<LoggerSystemCollection>.Filter.And(
                Builders<LoggerSystemCollection>.Filter.Gte(u => u.UtcTimeStamp, request.DateBegin.Value),
                Builders<LoggerSystemCollection>.Filter.Lte(u => u.UtcTimeStamp, request.DateEnd.Value)
                );
        }

        var sort = Builders<LoggerSystemCollection>.Sort.Descending(x => x.UtcTimeStamp);

        var total = await systemLogMongoRepo.CountAsync(f, cancellationToken: cancellationToken);
        var logs = await systemLogMongoRepo.FindListByPageAsync(f, request.Page, request.Size, sort: sort, cancellationToken: cancellationToken);

        var results = mapper.Map<List<LoggerSystemPageResult>>(logs);
        return Result.Success(new PaginationResult<LoggerSystemPageResult>(results, total));
    }
}
