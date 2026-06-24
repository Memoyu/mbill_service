namespace Memo.Bill.Application.Ledgers.Commands;


[Authorize(Permissions = ApiPermission.Ledger.Join)]
public record JoinLedgerUserCommand(long LedgerId) : IAuthorizeableRequest<Result>;

public class JoinLedgerUserCommandValidator : AbstractValidator<JoinLedgerUserCommand>
{
    public JoinLedgerUserCommandValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotEmpty()
            .WithMessage("账本Id不能为空");
    }
}

public class JoinLedgerUserCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo
    ) : IRequestHandler<JoinLedgerUserCommand, Result>
{
    public async Task<Result> Handle(JoinLedgerUserCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var ledger = await ledgerRepo.Select.Where(l => l.LedgerId == request.LedgerId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账本不存在或已删除");
        if (ledger.CreateUserId == userId)
            throw new ApplicationException("你是账本的创建人，无需加入");
        if (await ledgerUserRepo.Select.AnyAsync(lu => lu.LedgerId == request.LedgerId && lu.UserId == userId, cancellationToken))
            throw new ApplicationException("你已是账本成员，无需加入");

        var entity = await ledgerUserRepo.InsertAsync(new LedgerUser { UserId = userId, LedgerId = ledger.LedgerId }, cancellationToken)
            ?? throw new ApplicationException("加入账本失败");

        return Result.Success();
    }
}
