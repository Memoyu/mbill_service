namespace Memo.Bill.Application.Accounts.Commands.Create;

public class CreateAccountCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<CreateAccountCommand, Result>
{
    public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}