namespace Memo.Bill.Application.Ledgers.Commands;

public record SortLedgerDto(long LedgerId, int Sort);

[Authorize(Permissions = ApiPermission.Ledger.Sort)]
public record SortLedgerCommand(List<SortLedgerDto> Sorts) : IAuthorizeableRequest<Result>;

public class SortLedgerCommandValidator : AbstractValidator<SortLedgerCommand>
{
    public SortLedgerCommandValidator()
    {
        RuleFor(x => x.Sorts)
            .NotEmpty()
            .WithMessage("排序账本不能为空");
    }
}


public class SortLedgerCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo
    ) : IRequestHandler<SortLedgerCommand, Result>
{
    public async Task<Result> Handle(SortLedgerCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        // 更新排序
        var ledgerUsers = await ledgerUserRepo.Select.Where(l => l.UserId == userId).ToListAsync(cancellationToken) ?? [];
        ledgerUsers.ForEach(l =>
        {
            l.Sort = request.Sorts.FirstOrDefault(s => s.LedgerId == l.LedgerId)?.Sort ?? l.Sort;
        });
        var rows = await ledgerUserRepo.UpdateAsync(ledgerUsers, cancellationToken);

        return rows > 0 ? Result.Success() : throw new ApplicationException("更新账本排序失败");
    }
}

