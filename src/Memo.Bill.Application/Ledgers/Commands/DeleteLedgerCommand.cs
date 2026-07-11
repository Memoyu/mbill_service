using SharpCompress.Common;

namespace Memo.Bill.Application.Ledgers.Commands;

[Authorize(Permissions = ApiPermission.Ledger.Delete)]
[Transactional]
public record DeleteLedgerCommand(long LedgerId) : IAuthorizeableRequest<Result>;

public class DeleteLedgerCommandValidator : AbstractValidator<DeleteLedgerCommand>
{
    public DeleteLedgerCommandValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotEmpty()
            .WithMessage("账本Id不能为空");
    }
}

public class DeleteLedgerCommanddHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo
    ) : IRequestHandler<DeleteLedgerCommand, Result>
{
    public async Task<Result> Handle(DeleteLedgerCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var ledger = await ledgerRepo.Select.Where(l => l.LedgerId == request.LedgerId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账本不存在或已删除");
        if (ledger.CreateUserId != userId)
            throw new ApplicationException("非账本创建者，无法删除");

        if (ledger.Default)
            throw new ApplicationException("默认账本无法删除");

        var rows = await ledgerRepo.DeleteAsync(ledger, cancellationToken);
        // 删除关联用户
        await ledgerUserRepo.DeleteAsync(l => l.LedgerId == ledger.LedgerId, cancellationToken);

        return rows > 0 ? Result.Success() : throw new ApplicationException("删除账本失败");
    }
}