namespace Memo.Bill.Application.Accounts.Commands.Update;

public class UpdateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<UpdateAccountCommand, Result>
{
    public async Task<Result> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}