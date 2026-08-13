using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 搜索账单
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.Search)]
public record SearchBillQuery : BillSearchRequest, IAuthorizeableRequest<Result>;

internal class SearchBillQueryHandler(IBillService billService) : IRequestHandler<SearchBillQuery, Result>
{
    public async Task<Result> Handle(SearchBillQuery request, CancellationToken cancellationToken)
    {
        var result = await billService.SearchPageAsync(request, cancellationToken);
        return Result.Success(result);
    }
}