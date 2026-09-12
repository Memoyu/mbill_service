using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

/// <summary>
/// 获取账单汇总账户
/// </summary>
[Authorize(Permissions = ApiPermission.Bill.SummaryAccount)]
public record SummaryBillAccountQuery : BillQueryRequest, IAuthorizeableRequest<Result>
{
}

public class SummaryBillAccountQueryValidator : AbstractValidator<SummaryBillAccountQuery>
{
    public SummaryBillAccountQueryValidator()
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

internal class SummaryBillAccountQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBillService billService,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<SummaryBillAccountQuery, Result>
{
    public async Task<Result> Handle(SummaryBillAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;
        var (begin, end) = (request.BeginDate!.Value.StartOfDay(), request.EndDate!.Value.EndOfDay());

        var result = new BillSummaryAccountResult();
        request.LedgerIds = await billService.FilterLedgerAsync(request.LedgerIds, cancellationToken);
        if (request.LedgerIds.Count < 1)
            return Result.Success(result);

        var bills = await billRepo.Select
            .Include(s => s.Account)
            .Where(s => s.CreateUserId == userId) // 统计时，只统计个人的
            .Where(s => request.LedgerIds.Contains(s.LedgerId))
            .Where(s => s.Date <= end && s.Date >= begin)
            .WhereIf(request.Type.HasValue, s => s.Type == request.Type)
            .ToListAsync(b => new { Bill = new BillAmountSummaryDto(b.BillId, b.Type, b.Amount, b.Date), b.Account }, cancellationToken);

        var bGroups = bills.GroupBy(b => b.Bill.Type).ToList();

        var totalExpend = 0M;
        var totalIncome = 0M;
        foreach (var bg in bGroups)
        {
            var acGroups = bg.GroupBy(b => b.Account.AccountId).ToList();
            foreach (var ag in acGroups)
            {
                var ags = ag.ToList();
                var account = ags.First().Account;
                var amount = ags.Sum(b => b.Bill.Amount);
                var item = new BillSummaryAccountItem
                {
                    AccountId = account.AccountId,
                    Name = account.Name,
                    Icon = account.Icon,
                    Count = ags.Count,
                    Amount = amount,
                };

                if (bg.Key == BillType.Expend)
                {
                    totalExpend += amount;
                    result.Expends.Add(item);
                }
                else
                {
                    totalIncome += amount;
                    result.Incomes.Add(item);
                }
            }
        }

        result.Expends.ForEach(c =>
        {
            c.Percent = (c.Amount / totalExpend).ToRound(2) * 100;
        });
        result.Incomes.ForEach(c =>
        {
            c.Percent = (c.Amount / totalIncome).ToRound(2) * 100;
        });

        return Result.Success(result);
    }
}