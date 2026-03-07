namespace Memo.Bill.Application.Bills.Queries.Get;

public class GetBillQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billingRepo
    ) : IRequestHandler<GetBillQuery, Result>
{
    public async Task<Result> Handle(GetBillQuery request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
