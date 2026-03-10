using Memo.Bill.Application.Loggers.Common;
using Memo.Bill.Domain.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Memo.Bill.Application.Loggers.Queries.System;

[Authorize(Permissions = ApiPermission.LoggerSystem.Get)]
public record GetLoggerSystemQuery(string LogId) : IAuthorizeableRequest<Result>;

public class GetLoggerSystemQueryValidator : AbstractValidator<GetLoggerSystemQuery>
{
    public GetLoggerSystemQueryValidator()
    {
        RuleFor(x => x.LogId)
            .NotEmpty()
            .WithMessage("系统日志Id不能为空");
    }
}

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
