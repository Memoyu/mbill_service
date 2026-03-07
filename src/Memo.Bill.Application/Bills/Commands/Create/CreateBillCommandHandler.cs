namespace Memo.Bill.Application.Bills.Commands.Create;

public class CreateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billingRepo
    ) : IRequestHandler<CreateBillCommand, Result>
{
    public async Task<Result> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}