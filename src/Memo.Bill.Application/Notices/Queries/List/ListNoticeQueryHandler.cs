namespace Memo.Bill.Application.Notices.Queries.List;

public class ListNoticeQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<ListNoticeQuery, Result>
{
    public async Task<Result> Handle(ListNoticeQuery request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
