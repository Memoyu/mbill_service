namespace Memo.Bill.Application.Notices.Queries.Get;

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
