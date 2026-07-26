using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.RefundCreate)]
[Transactional]
public record CreateRefundBillCommand(
    long BillId,
    long AccountId,
    decimal Amount,
    DateTime Date,
    string? Remark
    ) : IAuthorizeableRequest<Result>;

public class RefundBillCommandValidator : AbstractValidator<CreateRefundBillCommand>
{
    public RefundBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("退回账户Id不能为空");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("退款金额不能为0");
    }
}

public class RefundBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<CreateRefundBillCommand, Result>
{
    public async Task<Result> Handle(CreateRefundBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
          ?? throw new ApplicationException("账单不存在或已删除");
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
         ?? throw new ApplicationException("账户不存在或已删除");

        var refund = await billRefundRepo.InsertAsync(new BillRefund
        {
            BillId = bill.BillId,
            AccountId = account.AccountId,
            Amount = request.Amount,
            AmountBefore = bill.Amount,
            Remark = request.Remark ?? string.Empty,
            Date = request.Date
        }, cancellationToken);

        if (refund.Id <= 0)
            throw new ApplicationException("新增账单退款失败");

        bill.Amount -= request.Amount;
        await billRepo.UpdateAsync(bill, cancellationToken);

        var dto = mapper.Map<BillRefundResult>(refund);
        dto.Account = mapper.Map<AccountBaseResult>(account);
        // 账户补全
        if (dto.Account.ParentId.HasValue)
            dto.Account.Parent = await accountRepo.Select.Where(t => dto.Account.ParentId == t.AccountId).FirstAsync(cancellationToken);
        return Result.Success(dto);
    }
}