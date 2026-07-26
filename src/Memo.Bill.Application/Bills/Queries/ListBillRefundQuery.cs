using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

[Authorize(Permissions = ApiPermission.Bill.RefundList)]
public record ListBillRefundQuery(long BillId) : IAuthorizeableRequest<Result>;

public class ListBillRefundQueryValidator : AbstractValidator<ListBillRefundQuery>
{
    public ListBillRefundQueryValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");
    }
}

public class ListBillRefundQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<ListBillRefundQuery, Result>
{
    public async Task<Result> Handle(ListBillRefundQuery request, CancellationToken cancellationToken)
    {
        var refunds = await billRefundRepo.Select
            .Include(br => br.Account)
            .Where(t => t.BillId == request.BillId)
            .OrderByDescending(t => t.CreateTime)
            .ToListAsync(cancellationToken);

        var dtos = new List<BillRefundResult>();
        if (refunds.Count > 0)
        {
            var parAcIds = refunds.Where(r => r.Account.ParentId.HasValue).Select(r => r.Account.ParentId).Distinct().ToList();
            var parAcs = await accountRepo.Select.Where(t => parAcIds.Contains(t.AccountId)).ToListAsync(cancellationToken);
            dtos = mapper.Map<List<BillRefundResult>>(refunds);
            dtos.ForEach(b =>
            {
                b.Account.Parent = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            });
        }

        return Result.Success(dtos);
    }
}