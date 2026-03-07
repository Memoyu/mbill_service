namespace Memo.Bill.Application.Bills.Commands.Delete;

public class DeleteBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billingRepo
    ) : IRequestHandler<DeleteBillCommand, Result>
{
    public async Task<Result> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}