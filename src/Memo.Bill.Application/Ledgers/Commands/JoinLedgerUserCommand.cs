using Memo.Bill.Application.Ledgers.Common;
using Memo.Bill.Application.Users.Common;
using Memo.Bill.Domain.Events.Ledgers;

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
    IPublisher publisher,
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<JoinLedgerUserCommand, Result>
{
    public async Task<Result> Handle(JoinLedgerUserCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var ledger = await ledgerRepo.Select.Where(l => l.LedgerId == request.LedgerId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账本不存在或已删除");

        var ledgerUsers = await ledgerUserRepo.Select.Include(l => l.User).Where(l => l.LedgerId == ledger.LedgerId).ToListAsync(cancellationToken) ?? [];
        if (ledgerUsers.Any(lu => lu.UserId == userId))
            throw new ApplicationException("你已是账本成员，无需再加入");

        // 使用默认颜色
        await publisher.Publish(new CreateLedgerEvent(ledger.LedgerId, userId, 0), cancellationToken);

        var dto = mapper.Map<LedgerResult>(ledger);
        dto.Expend = await billRepo.Select.Where(b => b.LedgerId == ledger.LedgerId && b.Type == BillType.Expend).SumAsync(b => b.Amount, cancellationToken);
        dto.Income = await billRepo.Select.Where(b => b.LedgerId == ledger.LedgerId && b.Type == BillType.Income).SumAsync(b => b.Amount, cancellationToken);
        dto.Users = mapper.Map<List<UserBaseResult>>(ledgerUsers.Where(l => l.UserId != userId).Select(l => l.User));

        return Result.Success(dto);
    }
}
