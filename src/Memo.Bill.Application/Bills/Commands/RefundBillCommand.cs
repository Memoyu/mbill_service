namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Refund)]
[Transactional]
public record RefundBillCommand(
    long BillId,
    long AccountId,
    decimal Amount,
    DateTime Date,
    string? Remark
    ) : IAuthorizeableRequest<Result>;

public class RefundBillCommandValidator : AbstractValidator<RefundBillCommand>
{
    public RefundBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("金额不能为0");
    }
}

public class RefundBillCommandHandler(
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<RefundBillCommand, Result>
{
    public async Task<Result> Handle(RefundBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
          ?? throw new ApplicationException("账单不存在或已删除");
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
         ?? throw new ApplicationException("账户不存在或已删除");

        bill.Amount -= request.Amount;
        await billRepo.UpdateAsync(bill, cancellationToken);

        var refund = await billRefundRepo.InsertAsync(new BillRefund
        {
            BillId = bill.BillId,
            AccountId = account.AccountId,
            Amount = request.Amount,
            Remark = request.Remark ?? string.Empty,
            Date = request.Date
        }, cancellationToken);

        return refund.Id > 0 ? Result.Success(bill.BillId) : throw new ApplicationException("新增账单退款失败");
    }
}