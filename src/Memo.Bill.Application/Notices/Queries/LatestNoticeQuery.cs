namespace Memo.Bill.Application.Notices.Queries;

public record LatestNoticeQuery() : IAuthorizeableRequest<Result>;

public class LatestNoticeQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<LatestNoticeQuery, Result>
{
    public async Task<Result> Handle(LatestNoticeQuery request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
