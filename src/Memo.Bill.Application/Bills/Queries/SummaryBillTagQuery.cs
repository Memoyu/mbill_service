using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单汇总账户
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SummaryAccount)]
public record SummaryBillTagQuery : BillQueryRequest, IAuthorizeableRequest<Result>
{
}

public class SummaryBillTagQueryValidator : AbstractValidator<SummaryBillTagQuery>
{
    public SummaryBillTagQueryValidator()
    {
        RuleFor(x => x.BeginDate)
            .NotEmpty()
            .WithMessage("开始时间不能为空");

        RuleFor(x => x.EndDate)
             .NotEmpty()
            .WithMessage("结束时间不能为空");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.BeginDate).WithMessage("结束时间必须晚于开始时间");
    }
}

internal class SummaryBillTagQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBillService billService,
    IBaseDefaultRepository<Billing> billRepo,
     IBaseDefaultRepository<BillTag> billTagRepo
    ) : IRequestHandler<SummaryBillTagQuery, Result>
{
    public async Task<Result> Handle(SummaryBillTagQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate!.Value.StartOfDay(), request.EndDate!.Value.EndOfDay());

        var result = new BillSummaryTagResult();
        request.LedgerIds = await billService.FilterLedgerAsync(request.LedgerIds, cancellationToken);
        if (request.LedgerIds.Count < 1)
            return Result.Success(result);

        var bills = await billRepo.Select
            .Where(s => s.CreateUserId == userId) // 统计时，只统计个人的
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .Where(s => s.Date <= end && s.Date >= begin)
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .ToListAsync(b => new BillAmountSummaryDto(b.BillId, b.Type, b.Amount, b.Date), cancellationToken);

        var billIds = bills.Select(b => b.BillId).ToList();
        var billTags = await billTagRepo.Select
            .Include(bt => bt.Tag)
            .Where(bt => billIds.Contains(bt.BillId))
            .ToListAsync(cancellationToken);

        var btGroups = billTags.GroupBy(bt => bt.TagId);

        var tags = new List<BillSummaryTagItem>();
        foreach (var btg in btGroups)
        {
            var bts = btg.ToList();
            var tag = bts.First().Tag;
            var bs = bills.Where(b => bts.Any(bt => bt.BillId == b.BillId)).ToList();

            var expend = 0M;
            var income = 0M;
            var expendCount = 0;
            var incomeCount = 0;
            foreach (var b in bs)
            {
                if (b.Type == BillType.Expend)
                {
                    expend += b.Amount;
                    expendCount += 1;
                }
                else
                {
                    income += b.Amount;
                    incomeCount += 1;
                }
            }

            tags.Add(new BillSummaryTagItem
            {
                TagId = tag.TagId,
                Name = tag.Name,
                Expend = expend,
                Income = income,
                ExpendCount = expendCount,
                IncomeCount = incomeCount
            });
        }
        result.Tags = tags;
        return Result.Success(result);
    }
}