using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取搜索账单记录
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SearchRecord)]
public record SearchBillRecordQuery : IAuthorizeableRequest<Result>;

internal class SearchBillRecordQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<BillSearchRecord> billSearchRecordRepo
    ) : IRequestHandler<SearchBillRecordQuery, Result>
{
    public async Task<Result> Handle(SearchBillRecordQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var end = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        var begin = end.AddMonths(-3);

        var records = await billSearchRecordRepo.Select
            .Where(x => x.CreateTime >= begin && x.CreateTime <= end && x.CreateUserId == userId).ToListAsync(cancellationToken);


        return Result.Success(mapper.Map<BillSearchRecordResult>(records));
    }
}