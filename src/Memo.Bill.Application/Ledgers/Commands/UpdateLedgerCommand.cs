namespace Memo.Bill.Application.Ledgers.Commands;

[Authorize(Permissions = ApiPermission.Ledger.Update)]
public record UpdateLedgerCommand(long LedgerId, string Name, int Color) : IAuthorizeableRequest<Result>;

public class UpdateLedgerCommandValidator : AbstractValidator<UpdateLedgerCommand>
{
    public UpdateLedgerCommandValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotEmpty()
            .WithMessage("账本Id不能为空");

        RuleFor(x => x.Name)
          .NotEmpty()
          .WithMessage("账本名称不能为空");
    }
}

public class UpdateLedgerCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo
    ) : IRequestHandler<UpdateLedgerCommand, Result>
{
    public async Task<Result> Handle(UpdateLedgerCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entity = await ledgerRepo.Select.Where(x => x.LedgerId == request.LedgerId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账本不存在或已删除");

        entity.Color = request.Color;
        entity.Name = request.Name;
        var rows = await ledgerRepo.UpdateAsync(entity, cancellationToken);

        return rows > 0 ? Result.Success() : throw new ApplicationException("更新账本失败");
    }
}

