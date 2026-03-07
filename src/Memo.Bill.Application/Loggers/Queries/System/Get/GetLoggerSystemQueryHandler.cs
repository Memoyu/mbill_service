using Memo.Bill.Application.Loggers.Common;
using Memo.Bill.Domain.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Memo.Bill.Application.Logger.Queries.System.Get;

public class GetLoggerSystemQueryHandler(
    IMapper mapper,
    IBaseMongoRepository<LoggerSystemCollection> systemLogMongoRepo) : IRequestHandler<GetLoggerSystemQuery, Result>
{
    public async Task<Result> Handle(GetLoggerSystemQuery request, CancellationToken cancellationToken)
    {
        var f = Builders<LoggerSystemCollection>.Filter.Empty;
        f = Builders<LoggerSystemCollection>.Filter.Eq("_id", new ObjectId(request.LogId));

        var logs = await systemLogMongoRepo.FindListAsync(f, cancellationToken: cancellationToken) ?? throw new ApplicationException("系统日志不存在");

        var dto = logs.Count > 0 ? mapper.Map<LoggerSystemResult>(logs.FirstOrDefault()!) : throw new ApplicationException("系统日志不存在");

        return Result.Success(dto);
    }
}
