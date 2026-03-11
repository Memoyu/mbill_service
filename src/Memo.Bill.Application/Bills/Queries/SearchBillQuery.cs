using Memo.Bill.Application.Common.Security;
using Memo.Bill.Domain.Entities.Mongo;
using MongoDB.Driver;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 搜索账单
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SearchRecord)]
public record SearchBillQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    /// <summary>
    /// 账单类型
    /// </summary>
    public List<BillType>? Types { get; set; }

    /// <summary>
    /// 账单分类Id
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 账单账户Id
    /// </summary>
    public long? AccountId { get; set; }

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
    public DateTime? DateBegin { get; set; }

    /// <summary>
    /// 账单时间截止
    /// </summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string? KeyWord { get; set; }
}

internal class SearchBillQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<BillSearchRecord> billSearchRecordRepo,
    IBaseMongoRepository<BillingCollection> billMongoRepo
    ) : IRequestHandler<SearchBillQuery, Result>
{
    public async Task<Result> Handle(SearchBillQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var record = mapper.Map<BillSearchRecord>(request);
        await billSearchRecordRepo.InsertAsync(record, cancellationToken);


        //var sort = Builders<BillingCollection>.Sort.Descending("Time");
        //var bFilter = Builders<BillingCollection>.Filter;
        //List<FilterDefinition<BillingCollection>> filters = new List<FilterDefinition<BillingCollection>>();

        //// 账单类型
        //if (request.Types != null && request.Types.Any())
        //    filters.Add(bFilter.And(bFilter.In(b => b.Type, request.Types)));

        //// 账单分类
        //if (request.CategoryBIds != null && request.CategoryBIds.Any())
        //    filters.Add(bFilter.And(bFilter.In(b => b.CategoryBId, request.CategoryBIds)));

        //// 账单账户
        //if (request.AssetBIds != null && request.AssetBIds.Any())
        //    filters.Add(bFilter.And(bFilter.In(b => b.AssetBId, request.AssetBIds)));

        //// 金额区间
        //if (request.Amount != null)
        //{
        //    if (request.Amount.Max.HasValue && !request.Amount.Min.HasValue)
        //        filters.Add(bFilter.And(bFilter.Lte(b => b.Amount, request.Amount.Max.Value)));
        //    else if (!request.Amount.Max.HasValue && request.Amount.Min.HasValue)
        //        filters.Add(bFilter.And(bFilter.Gte(b => b.Amount, request.Amount.Min.Value)));
        //    else if (request.Amount.Max.HasValue && request.Amount.Min.HasValue)
        //        filters.Add(bFilter.And(bFilter.Gte(b => b.Amount, request.Amount.Min.Value), bFilter.Lte(b => b.Amount, request.Amount.Max.Value)));
        //}
        //// 金额区间
        //if (request.Date != null && request.Date.Begin.HasValue && request.Date.End.HasValue)
        //    filters.Add(bFilter.And(bFilter.Gte(b => b.Time, request.Date.Begin.Value), bFilter.Lte(b => b.Time, request.Date.End.Value.AddDays(1).AddSeconds(-1))));

        //// 地址
        //if (!string.IsNullOrWhiteSpace(request.Address))
        //    filters.Add(bFilter.And(bFilter.Where(b => b.Address.Contains(request.Address))));

        //// 备注
        //if (!string.IsNullOrWhiteSpace(request.Remark))
        //    filters.Add(bFilter.And(bFilter.Where(b => b.Description.Contains(request.Remark))));

        //// 关键词
        //if (string.IsNullOrWhiteSpace(request.Address) && string.IsNullOrWhiteSpace(request.Remark) && !string.IsNullOrWhiteSpace(request.KeyWord))
        //    filters.Add(bFilter.And(bFilter.Or(bFilter.Where(b => b.Address.Contains(request.KeyWord)),
        //        bFilter.Where(b => b.Description.Contains(request.KeyWord)))));

        //var filter = bFilter.And(bFilter.Eq(b => b.CreateUserBId, CurrentUser.BId), bFilter.And(filters));//时间段条件用OR拼在一起
        //var paged = new PagedDto<BillDetailDto>();

        //var total = await _mongoRepo.CountAsync(filter);
        //if (total != 0)
        //{
        //    var bills = await _mongoRepo.FindListByPageAsync(filter, request.Page, request.Size, null, sort);
        //    List<BillDetailDto> dtos = new List<BillDetailDto>();
        //    foreach (var bill in bills)
        //    {
        //        var category = await _categoryRepo.GetCategoryAsync(bill.CategoryBId);
        //        var asset = await _assetRepo.GetAssetAsync(bill.AssetBId);
        //        bill.Category = category;
        //        bill.Asset = asset;
        //        var dto = Mapper.Map<BillDetailDto>(bill);
        //        dtos.Add(dto);
        //    }

        //    paged.Total = total;
        //    paged.Items = dtos;
        //}

        return Result.Success();
    }
}