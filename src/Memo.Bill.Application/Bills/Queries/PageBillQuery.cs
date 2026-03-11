using Memo.Bill.Application.Common.Interfaces.Services.App;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单分页基础属性
/// </summary>
public record PageBillBaseQuery : PaginationQuery
{
    /// <summary>
    /// 指定的日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 查询时间类型：0-查当日，1-查询当月，2-查询当年
    /// </summary>
    public int DateType { get; set; }

    /// <summary>
    /// 账单类型
    /// </summary>
    public BillType? Type { get; set; }

    /// <summary>
    /// 账单分类
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 账单账户
    /// </summary>
    public long? AccountId { get; set; }
}


/// <summary>
/// 获取账单分页
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.Page)]
public record PageBillQuery : PageBillBaseQuery, IAuthorizeableRequest<Result>;

internal class PageBillQueryHandler(
    IBillService billService
    ) : IRequestHandler<PageBillQuery, Result>
{
    public async Task<Result> Handle(PageBillQuery request, CancellationToken cancellationToken)
    {
        var result = await billService.GetBillPageAsync(request, cancellationToken);
        return Result.Success(result);
    }
}
