using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Delete)]
[Transactional]
public record DeleteBillCommand(long BillId) : IAuthorizeableRequest<Result>;

public class DeleteBillCommandValidator : AbstractValidator<DeleteBillCommand>
{
    public DeleteBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");
    }
}

public class DeleteBillCommandHandler(
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<DeleteBillCommand, Result>
{
    public async Task<Result> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账单不存在或已删除");

        bill.AddDomainEvent(new DeleteBillEvent(bill.BillId));
        var row = await billRepo.DeleteAsync(bill, cancellationToken);

        return row > 0 ? Result.Success() : throw new ApplicationException("删除账单失败");
    }
}