using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取指定日期账单
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.GetDate)]
public record GetBillDateQuery : IAuthorizeableRequest<Result>
{
    /// <summary>
    /// 指定的日期
    /// </summary>
    public DateTime Date { get; set; }

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

internal class GetBillDateQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<GetBillDateQuery, Result>
{
    public async Task<Result> Handle(GetBillDateQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;

        var begin = request.Date.Date;
        var end = begin.AddDays(1).AddSeconds(-1);
        var bills = await billRepo
            .Select
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date >= begin && s.Date <= end)
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .WhereIf(request.CategoryId.HasValue, s => s.CategoryId == request.CategoryId)
            .WhereIf(request.AccountId.HasValue, s => s.AccountId == request.AccountId)
            .OrderBy("date DESC")
            .ToListAsync(cancellationToken);

        var expend = 0m;
        var income = 0m;
        foreach (var bill in bills)
        {
            if (bill.Type == BillType.Expend)
                expend += bill.Amount;
            else
                income += bill.Amount;
        }

        return Result.Success(new BillDateResult
        {
            Date = begin,
            Items = mapper.Map<List<BillResult>>(bills),
            Expend = expend.FormatAmount(),
            Income = income.FormatAmount(),
        });
    }
}
