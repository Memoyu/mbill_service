using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单排行榜
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.Ranking)]
public record RankingBillQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 账单类型
    /// </summary>
    public BillType Type { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 账户Id
    /// </summary>
    public long? AccountId { get; set; }
}

public class RankingBillQueryValidator : AbstractValidator<RankingBillQuery>
{
    public RankingBillQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("账单类型错误");
    }
}

internal class RankingBillQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<RankingBillQuery, Result>
{
    public async Task<Result> Handle(RankingBillQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate.FirstTimeOfDay(), request.EndDate.LastTimeOfDay());

        var bills = await billRepo.Select
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(s => s.CreateUserId == userId)
            .Where(s => s.Date <= end && s.Date >= begin)
            .WhereIf(request.Type == BillType.Expend, s => s.Type == BillType.Expend)
            .WhereIf(request.Type == BillType.Income, s => s.Type == BillType.Income)
            .WhereIf(request.CategoryId.HasValue && request.CategoryId != 0, s => s.CategoryId == request.CategoryId)
            .WhereIf(request.AccountId.HasValue && request.AccountId != 0, s => s.AccountId == request.AccountId)
            .OrderBy("amount DESC")
            .ToPageListAsync(request, out long total, cancellationToken);

        return Result.Success(new PaginationResult<BillResult>(mapper.Map<List<BillResult>>(bills), total));
    }
}