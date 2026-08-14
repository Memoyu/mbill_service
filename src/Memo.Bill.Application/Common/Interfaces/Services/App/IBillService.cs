using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Domain.Entities.Mongo;

namespace Memo.Bill.Application.Common.Interfaces.Services.App;

internal interface IBillService 
{
    /// <summary>
    /// 过滤账本
    /// </summary>
    /// <param name="ledgerIds">请求传入账本</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<long>> FilterLedgerAsync(List<long>? ledgerIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// 账单分页
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaginationResult<BillPageItemResult>> PageAsync(BillQueryRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// 账单搜索
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaginationResult<BillingCollection>> SearchAsync(BillSearchRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// 账单搜索分页（分页补全实体数据）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaginationResult<BillPageItemResult>> SearchPageAsync(BillSearchRequest request, CancellationToken cancellationToken);
}
