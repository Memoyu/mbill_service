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
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<DeleteBillCommand, Result>
{
    public async Task<Result> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}