namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单分页基础属性
/// </summary>
public record PageBillBaseQuery : PaginationQuery
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

public class PageBillQueryValidator : AbstractValidator<PageBillQuery>
{
    public PageBillQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

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
