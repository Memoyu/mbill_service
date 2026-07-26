using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Queries;

[Authorize(Permissions = ApiPermission.Bill.RefundGet)]
public record GetBillRefundQuery(long RefundId) : IAuthorizeableRequest<Result>;

public class GetBillRefundQueryValidator : AbstractValidator<GetBillRefundQuery>
{
    public GetBillRefundQueryValidator()
    {
        RuleFor(x => x.RefundId)
            .NotEmpty()
            .WithMessage("退款Id不能为空");
    }
}

public class GetBillRefundQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<GetBillRefundQuery, Result>
{
    public async Task<Result> Handle(GetBillRefundQuery request, CancellationToken cancellationToken)
    {
        var refund = await billRefundRepo.Select.Where(t => t.RefundId == request.RefundId).FirstAsync(cancellationToken)
         ?? throw new ApplicationException("账单退款不存在或已删除");

        var dto = mapper.Map<BillRefundResult>(refund);

        // 账户补全
        if (dto.Account.ParentId.HasValue)
            dto.Account.Parent = await accountRepo.Select.Where(t => dto.Account.ParentId == t.AccountId).FirstAsync(cancellationToken);

        return Result.Success(dto);
    }
}