namespace Memo.Bill.Application.Ledgers.Commands;

public record UpdateLedgerColorItem(long LedgerId, int Color);

[Authorize(Permissions = ApiPermission.Ledger.UpdateColor)]
public record UpdateLedgerColorCommand(List<UpdateLedgerColorItem> Items) : IAuthorizeableRequest<Result>;

public class UpdateLedgerColorCommandValidator : AbstractValidator<UpdateLedgerColorCommand>
{
    public UpdateLedgerColorCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("更新账本不能为空");
    }
}

public class UpdateLedgerColorCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo
    ) : IRequestHandler<UpdateLedgerColorCommand, Result>
{
    public async Task<Result> Handle(UpdateLedgerColorCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entities = await ledgerUserRepo.Select.Where(x => request.Items.Any(i => i.LedgerId == x.LedgerId) && x.UserId == userId).ToListAsync(cancellationToken) ?? [];

        if (entities.Count > 0)
        {
            foreach (var e in entities)
            {
                var item = request.Items.FirstOrDefault(i => i.LedgerId == e.LedgerId);
                if (item == null) continue;
                e.Color = item.Color;
            }

            var rows = await ledgerUserRepo.UpdateAsync(entities, cancellationToken);
            if (rows < 1) throw new ApplicationException("保存账本颜色失败");
        }
        return Result.Success();
    }
}

