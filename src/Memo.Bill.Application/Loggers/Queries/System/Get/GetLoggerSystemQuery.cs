namespace Memo.Bill.Application.Logger.Queries.System.Get;

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
