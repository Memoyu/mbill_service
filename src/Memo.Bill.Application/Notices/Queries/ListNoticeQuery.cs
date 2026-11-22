using Memo.Bill.Application.Notices.Common;

namespace Memo.Bill.Application.Notices.Queries;

[Authorize(Permissions = ApiPermission.Notice.List)]
public record ListNoticeQuery(  DateTime? BeginDate, DateTime? EndDate) : IAuthorizeableRequest<Result>;

public class ListNoticeQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Notice> noticeRepo
    ) : IRequestHandler<ListNoticeQuery, Result>
{
    public async Task<Result> Handle(ListNoticeQuery request, CancellationToken cancellationToken)
    {
        var noticies = await noticeRepo.Select
            .WhereIf(request.BeginDate.HasValue, n => n.CreateTime >= request.BeginDate!.Value)
            .WhereIf(request.EndDate.HasValue, n => n.CreateTime <= request.EndDate!.Value)
            .OrderByDescending(n => n.CreateTime)
            .ToListAsync(cancellationToken);

        return Result.Success(mapper.Map<List<NoticeResult>>(noticies));
    }
}
