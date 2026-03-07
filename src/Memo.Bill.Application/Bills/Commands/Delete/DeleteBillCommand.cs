namespace Memo.Bill.Application.Bills.Commands.Delete;

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
