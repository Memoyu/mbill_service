using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Categories.Common;
using Memo.Bill.Domain.Entities.Mongo;
using MongoDB.Driver;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 搜索账单
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.Search)]
public record SearchBillQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    /// <summary>
    /// 账单类型
    /// </summary>
    public List<BillType>? Types { get; set; }

    /// <summary>
    /// 账单分类Id
    /// </summary>
    public List<long>? CategoryIds { get; set; }

    /// <summary>
    /// 账单账户Id
    /// </summary>
    public List<long>? AccountIds { get; set; }

    /// <summary>
    /// 金额区间最小值
    /// </summary>
    public decimal? AmountMin { get; set; }

    /// <summary>
    /// 金额区间最大值
    /// </summary>
    public decimal? AmountMax { get; set; }

    /// <summary>
    /// 账单时间起始
    /// </summary>
    public DateTime? BeginDate { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string? KeyWord { get; set; }
}

internal class SearchBillQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<BillSearchRecord> billSearchRecordRepo,
    IBaseMongoRepository<BillingCollection> billMongoRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<SearchBillQuery, Result>
{
    public async Task<Result> Handle(SearchBillQuery request, CancellationToken cancellationToken)
    {
        // 记录搜索记录
        var userId = currentUserProvider.UserId;
        var record = mapper.Map<BillSearchRecord>(request);
        await billSearchRecordRepo.InsertAsync(record, cancellationToken);

        // 组装Mongo查询
        var sort = Builders<BillingCollection>.Sort.Descending("Date");
        var whereFilter = Builders<BillingCollection>.Filter;
        List<FilterDefinition<BillingCollection>> filters = new List<FilterDefinition<BillingCollection>>();

        // 账单类型
        if (request.Types?.Count > 0)
            filters.Add(whereFilter.And(whereFilter.In(b => b.Type, request.Types)));

        // 账单分类
        if (request.CategoryIds?.Count > 0)
            filters.Add(whereFilter.And(whereFilter.In(b => b.CategoryId, request.CategoryIds)));

        // 账单账户
        if (request.AccountIds?.Count > 0)
            filters.Add(whereFilter.And(whereFilter.In(b => b.AccountId, request.AccountIds)));

        // 金额区间
        // 有最大值，没有最小值
        if (request.AmountMax.HasValue && !request.AmountMin.HasValue)
            filters.Add(whereFilter.And(whereFilter.Lte(b => b.Amount, request.AmountMax.Value)));
        // 没有最大值，有最小值
        else if (!request.AmountMax.HasValue && request.AmountMin.HasValue)
            filters.Add(whereFilter.And(whereFilter.Gte(b => b.Amount, request.AmountMin.Value)));
        // 有最大值，有最小值
        else if (request.AmountMax.HasValue && request.AmountMin.HasValue)
            filters.Add(whereFilter.And(whereFilter.Gte(b => b.Amount, request.AmountMin.Value), whereFilter.Lte(b => b.Amount, request.AmountMax.Value)));

        // 时间区间
        // 有起始时间，没有截止时间
        if (request.BeginDate.HasValue && !request.EndDate.HasValue)
            filters.Add(whereFilter.And(whereFilter.Gte(b => b.Date, request.BeginDate.Value)));
        // 没有起始时间，有截止时间
        else if (!request.BeginDate.HasValue && request.EndDate.HasValue)
            filters.Add(whereFilter.And(whereFilter.Lte(b => b.Date, request.EndDate.Value.AddDays(1).AddSeconds(-1))));
        // 有起始时间，有截止时间
        else if (request.BeginDate.HasValue && request.EndDate.HasValue)
            filters.Add(whereFilter.And(whereFilter.Gte(b => b.Date, request.BeginDate.Value), whereFilter.Lte(b => b.Date, request.EndDate.Value.AddDays(1).AddSeconds(-1))));

        // 关键词
        if (!string.IsNullOrWhiteSpace(request.KeyWord))
        {
            filters.Add(whereFilter.And(whereFilter.Or(
                whereFilter.Where(b => b.Address.Contains(request.KeyWord)),
                whereFilter.Where(b => b.Remark.Contains(request.KeyWord))
            )));
        }

        var filter = whereFilter.And(whereFilter.Eq(b => b.CreateUserId, userId), whereFilter.And(filters));//时间段条件用OR拼在一起
        var dtos = new List<BillResult>();
        var pageRes = new PaginationResult<BillResult>(dtos, 0);
        var total = await billMongoRepo.CountAsync(filter, null, cancellationToken);
        if (total != 0)
        {
            var bills = await billMongoRepo.FindListByPageAsync(filter, request.Page, request.Size, null, sort, cancellationToken);
            foreach (var bill in bills)
            {
                var dto = mapper.Map<BillResult>(bill);
                var category = await categoryRepo.Select.Where(c => c.CategoryId == bill.CategoryId).FirstAsync(cancellationToken);
                var account = await accountRepo.Select.Where(c => c.AccountId == bill.AccountId).FirstAsync(cancellationToken);          
                dto.Category = mapper.Map<CategoryBaseResult>(category);
                dto.Account = mapper.Map<AccountBaseResult>(account);
                dtos.Add(dto);
            }

            pageRes.Total = total;
            pageRes.Items = dtos;
        }

        return Result.Success(pageRes);
    }
}