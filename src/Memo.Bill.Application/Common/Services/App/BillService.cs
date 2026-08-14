using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Common.Interfaces.Services.Text;
using Memo.Bill.Application.Tags.Common;
using Memo.Bill.Domain.Entities.Mongo;
using MongoDB.Driver;

namespace Memo.Bill.Application.Common.Services.App;

[AppService]
internal class BillService(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    ISegmenterService segmenterService,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<BillTag> billTagRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo,
    IBaseMongoRepository<BillingCollection> billMongoRepo
    ) : IBillService
{
    public async Task<List<long>> FilterLedgerAsync(List<long>? ledgerIds, CancellationToken cancellationToken = default)
    {
        var userId = currentUserProvider.UserId;

        // 账本过滤，不传入账本时，查询当前用户所有
        ledgerIds = ledgerIds ?? [];
        return await ledgerUserRepo.Select
            .Where(l => l.UserId == userId)
            .WhereIf(ledgerIds.Count > 0, l => ledgerIds.Contains(l.LedgerId))
            .ToListAsync(l => l.LedgerId, cancellationToken);
    }

    public async Task<PaginationResult<BillPageItemResult>> PageAsync(BillQueryRequest request, CancellationToken cancellationToken = default)
    {
        // 账本为空，则不需要继续进行查询
        request.LedgerIds = await FilterLedgerAsync(request.LedgerIds, cancellationToken);
        if (request.LedgerIds.Count < 1)
            return new PaginationResult<BillPageItemResult>();

        var (begin, end) = (request.BeginDate?.FirstTimeOfDay(), request.EndDate?.LastTimeOfDay());
        // 标签过滤
        var tagBillIds = new List<long>();
        var tagIds = request.TagIds ?? [];
        if (tagIds.Count > 0)
        {
            tagBillIds = await billTagRepo.Select
                .Where(t => tagIds.Contains(t.TagId)).ToListAsync(t => t.BillId, cancellationToken);
        }

        // 排序
        var sort = string.IsNullOrWhiteSpace(request.Sort) ? "date DESC" : request.Sort;
        var bills = await billRepo
            .Select
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .WhereIf(begin.HasValue, s => s.Date >= begin!.Value)
            .WhereIf(end.HasValue, s => s.Date <= end!.Value)
            .WhereIf(tagBillIds.Count > 0, s => tagBillIds.Contains(s.BillId))
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .WhereIf((request.CategoryIds ?? []).Count > 0, s => request.CategoryIds!.Contains(s.CategoryId))
            .WhereIf((request.AccountIds ?? []).Count > 0, s => request.AccountIds!.Contains(s.AccountId))
            .OrderBy(sort)
            .ToPageListAsync(request, out var total, cancellationToken);

        var billIds = new HashSet<long>();
        var parCaIds = new HashSet<long>();
        var parAcIds = new HashSet<long>();

        foreach (var rb in bills)
        {
            billIds.Add(rb.BillId);

            if (rb.Category.ParentId.HasValue)
                parCaIds.Add(rb.Category.ParentId.Value);
            if (rb.Account.ParentId.HasValue)
                parAcIds.Add(rb.Account.ParentId.Value);
        }

        var parCas = await categoryRepo.Select.Where(t => parCaIds.Contains(t.CategoryId)).ToListAsync(cancellationToken);
        var parAcs = await accountRepo.Select.Where(t => parAcIds.Contains(t.AccountId)).ToListAsync(cancellationToken);
        var tags = await billTagRepo.Select.Include(t => t.Tag).Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);
        var refunds = await billRefundRepo.Select.Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<BillPageItemResult>>(bills);
        dtos.ForEach(b =>
        {
            b.Category.Parent = parCas.FirstOrDefault(c => c.CategoryId == b.Category.ParentId);
            b.Account.Parent = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            b.Tags = [.. tags.Where(t => t.BillId == b.BillId).Select(t => mapper.Map<TagBaseResult>(t.Tag))];
            b.RefundAmount = refunds.Where(r => r.BillId == b.BillId).Sum(r => r.Amount);
        });

        return new PaginationResult<BillPageItemResult>(dtos, total);
    }

    public async Task<PaginationResult<BillingCollection>> SearchAsync(BillSearchRequest request, CancellationToken cancellationToken)
    {
        // 账本为空，则不需要继续进行查询
        request.LedgerIds = await FilterLedgerAsync(request.LedgerIds, cancellationToken);
        if (request.LedgerIds.Count < 1)
            return new PaginationResult<BillingCollection>();

        // 组装Mongo查询
        var sort = Builders<BillingCollection>.Sort.Descending("Date");

        // 账本
        var f = Builders<BillingCollection>.Filter.In(b => b.Ledger.LedgerId, request.LedgerIds);

        #region 与条件

        // 排除账单
        if (request.ExcludeBillIds?.Count > 0)
            f &= Builders<BillingCollection>.Filter.Nin(b => b.BillId, request.ExcludeBillIds);

        // 类型
        if (request.Type.HasValue)
            f &= Builders<BillingCollection>.Filter.Eq(b => b.Type, request.Type.Value);

        // 金额区间
        // 有最大值，没有最小值
        if (request.AmountMax.HasValue && !request.AmountMin.HasValue)
            f &= Builders<BillingCollection>.Filter.Lte(b => b.Amount, request.AmountMax.Value);
        // 没有最大值，有最小值
        else if (!request.AmountMax.HasValue && request.AmountMin.HasValue)
            f &= Builders<BillingCollection>.Filter.Gte(b => b.Amount, request.AmountMin.Value);
        // 有最大值，有最小值
        else if (request.AmountMax.HasValue && request.AmountMin.HasValue)
            f &= Builders<BillingCollection>.Filter.And(
                Builders<BillingCollection>.Filter.Gte(b => b.Amount, request.AmountMin.Value),
                Builders<BillingCollection>.Filter.Lte(b => b.Amount, request.AmountMax.Value)
            );

        // 关键词
        if (!string.IsNullOrWhiteSpace(request.Keyword))
            f &= Builders<BillingCollection>.Filter.And(Builders<BillingCollection>.Filter.Text(segmenterService.CutWithSplitForSearch(request.Keyword)));

        // 时间区间
        // 有起始时间，没有截止时间
        if (request.BeginDate.HasValue && !request.EndDate.HasValue)
            Builders<BillingCollection>.Filter.Gte(b => b.Date, request.BeginDate.Value);
        // 没有起始时间，有截止时间
        else if (!request.BeginDate.HasValue && request.EndDate.HasValue)
            Builders<BillingCollection>.Filter.Lte(b => b.Date, request.EndDate.Value.AddDays(1).AddSeconds(-1));
        // 有起始时间，有截止时间
        else if (request.BeginDate.HasValue && request.EndDate.HasValue)
            Builders<BillingCollection>.Filter.And(
                Builders<BillingCollection>.Filter.Gte(b => b.Date, request.BeginDate.Value),
                Builders<BillingCollection>.Filter.Lte(b => b.Date, request.EndDate.Value.AddDays(1).AddSeconds(-1))
            );

        #endregion

        #region 或条件

        // 分类
        if (request.CategoryIds?.Count > 0)
            f |= Builders<BillingCollection>.Filter.In(b => b.Category.CategoryId, request.CategoryIds);

        // 账户
        if (request.AccountIds?.Count > 0)
            f |= Builders<BillingCollection>.Filter.In(b => b.Account.AccountId, request.AccountIds);

        // 标签
        if (request.TagIds?.Count > 0)
            f |= Builders<BillingCollection>.Filter.ElemMatch(b => b.Tags, Builders<BillingCollTag>.Filter.In(f => f.TagId, request.TagIds));

        #endregion

        var dtos = new List<BillingCollection>();
        var result = new PaginationResult<BillingCollection>(dtos, 0);
        var total = await billMongoRepo.CountAsync(f, null, cancellationToken);
        if (total != 0)
        {
            result.Total = total;
            result.Items = await billMongoRepo.FindListByPageAsync(f, request.Page, request.Size, null, sort, cancellationToken);
        }

        return result;
    }

    public async Task<PaginationResult<BillPageItemResult>> SearchPageAsync(BillSearchRequest request, CancellationToken cancellationToken)
    {
        var data = await SearchAsync(request, cancellationToken);
        var bills = data.Items;
        var billIds = new HashSet<long>();
        var parCaIds = new HashSet<long>();
        var parAcIds = new HashSet<long>();

        foreach (var rb in bills)
        {
            billIds.Add(rb.BillId);

            if (rb.Category.ParentId.HasValue)
                parCaIds.Add(rb.Category.ParentId.Value);
            if (rb.Account.ParentId.HasValue)
                parAcIds.Add(rb.Account.ParentId.Value);
        }

        var parCas = await categoryRepo.Select.Where(t => parCaIds.Contains(t.CategoryId)).ToListAsync(cancellationToken);
        var parAcs = await accountRepo.Select.Where(t => parAcIds.Contains(t.AccountId)).ToListAsync(cancellationToken);
        var tags = await billTagRepo.Select.Include(t => t.Tag).Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);
        var refunds = await billRefundRepo.Select.Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<BillPageItemResult>>(bills);
        dtos.ForEach(b =>
        {
            b.Category.Parent = parCas.FirstOrDefault(c => c.CategoryId == b.Category.ParentId);
            b.Account.Parent = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            b.Tags = [.. tags.Where(t => t.BillId == b.BillId).Select(t => mapper.Map<TagBaseResult>(t.Tag))];
            b.RefundAmount = refunds.Where(r => r.BillId == b.BillId).Sum(r => r.Amount);
        });

        return new PaginationResult<BillPageItemResult>(dtos, data.Total);
    }
}
