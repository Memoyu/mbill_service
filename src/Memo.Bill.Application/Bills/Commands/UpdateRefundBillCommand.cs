using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Bills.Common;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.RefundUpdate)]
[Transactional]
public record UpdateRefundBillCommand(
    long RefundId,
    long AccountId,
    decimal Amount,
    DateTime Date,
    string? Remark
    ) : IAuthorizeableRequest<Result>;

public class UpdateRefundBillCommandValidator : AbstractValidator<UpdateRefundBillCommand>
{
    public UpdateRefundBillCommandValidator()
    {
        RuleFor(x => x.RefundId)
            .NotEmpty()
            .WithMessage("退款Id不能为空");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("退回账户Id不能为空");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("退款金额不能为0");
    }
}

public class UpdateRefundBillCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<UpdateRefundBillCommand, Result>
{
    public async Task<Result> Handle(UpdateRefundBillCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.UserId;

        var refund = await billRefundRepo.Select.Where(t => t.RefundId == request.RefundId).FirstAsync(cancellationToken)
         ?? throw new ApplicationException("账单退款不存在或已删除");
        if (refund.CreateUserId != userId)
            throw new ApplicationException("非账单退款创建人，无法更新");
        var bill = await billRepo.Select.Where(t => t.BillId == refund.BillId).FirstAsync(cancellationToken)
          ?? throw new ApplicationException("账单不存在或已删除");
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
         ?? throw new ApplicationException("账户不存在或已删除");

        // 计算退款差额
        var diff = refund.Amount - request.Amount;

        // 赋值退款信息
        refund.AccountId = account.AccountId;
        refund.Amount = request.Amount;
        // refund.AmountBefore = bill.Amount; // 提交退款时的金额应该保持不变
        refund.Remark = request.Remark ?? string.Empty;
        refund.Date = request.Date;
        var row = await billRefundRepo.UpdateAsync(refund, cancellationToken);
        if (row < 1)
            throw new ApplicationException("更新账单退款失败");

        // 更新账单金额
        bill.Amount += diff;
        await billRepo.UpdateAsync(bill, cancellationToken);

        var dto = mapper.Map<BillRefundResult>(refund);
        dto.Account = mapper.Map<AccountBaseResult>(account);
        // 账户补全
        if (dto.Account.ParentId.HasValue)
            dto.Account.Parent = await accountRepo.Select.Where(t => dto.Account.ParentId == t.AccountId).FirstAsync(cancellationToken);
        return Result.Success(dto);
    }
}