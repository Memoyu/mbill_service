namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.RefundDelete)]
[Transactional]
public record DeleteRefundBillCommand(long RefundId ) : IAuthorizeableRequest<Result>;

public class DeleteRefundBillCommandValidator : AbstractValidator<DeleteRefundBillCommand>
{
    public DeleteRefundBillCommandValidator()
    {
        RuleFor(x => x.RefundId)
            .NotEmpty()
            .WithMessage("退款Id不能为空");
    }
}

public class DeleteRefundBillCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<DeleteRefundBillCommand, Result>
{
    public async Task<Result> Handle(DeleteRefundBillCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;

        var refund = await billRefundRepo.Select.Where(t => t.RefundId == request.RefundId).FirstAsync(cancellationToken)
         ?? throw new ApplicationException("账单退款不存在或已删除");
        if (refund.CreateUserId != userId)
            throw new ApplicationException("非账单退款创建人，无法删除");
        var bill = await billRepo.Select.Where(t => t.BillId == refund.BillId).FirstAsync(cancellationToken)
          ?? throw new ApplicationException("账单不存在或已删除");

        // 恢复账单金额
        bill.Amount += refund.Amount;
        await billRepo.UpdateAsync(bill, cancellationToken);

        var row = await billRefundRepo.DeleteAsync(refund, cancellationToken);
        return row > 0 ? Result.Success(refund.RefundId) : throw new ApplicationException("删除账单退款失败");
    }
}