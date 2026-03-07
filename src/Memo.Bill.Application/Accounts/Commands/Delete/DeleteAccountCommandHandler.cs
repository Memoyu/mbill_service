namespace Memo.Bill.Application.Accounts.Commands.Delete;

public class DeleteAccountCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<DeleteAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}