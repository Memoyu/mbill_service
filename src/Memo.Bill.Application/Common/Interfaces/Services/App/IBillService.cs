using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Bills.Queries;

namespace Memo.Bill.Application.Common.Interfaces.Services.App;

internal interface IBillService 
{
    /// <summary>
    /// 获取账单分页
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BillPageResult<BillResult>> GetBillPageAsync(PageBillBaseQuery query, CancellationToken cancellationToken);
}
