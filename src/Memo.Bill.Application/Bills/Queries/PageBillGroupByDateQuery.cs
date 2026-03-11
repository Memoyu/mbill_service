using Memo.Bill.Application.Common.Interfaces.Services.App;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单分页日期分组
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.PageGroupDate)]
public record PageBillGroupByDateQuery : PageBillBaseQuery, IAuthorizeableRequest<Result>;

internal class PageBillGroupByDateQueryHandler(
    IBillService billService
    ) : IRequestHandler<PageBillGroupByDateQuery, Result>
{
    public async Task<Result> Handle(PageBillGroupByDateQuery request, CancellationToken cancellationToken)
    {
        var result = await billService.GetBillPageAsync(request, cancellationToken);

        return Result.Success();
    }
}
