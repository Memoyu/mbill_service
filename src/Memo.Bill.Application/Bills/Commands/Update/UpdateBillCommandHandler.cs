namespace Memo.Bill.Application.Bills.Commands.Update;

public class UpdateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billingRepo
    ) : IRequestHandler<UpdateBillCommand, Result>
{
    public async Task<Result> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}