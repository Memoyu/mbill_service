namespace Memo.Bill.Application.Accounts.Queries.Get;

public class GetAccountQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<GetAccountQuery, Result>
{
    public async Task<Result> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
