using Memo.Bill.Application.Ledgers.Common;
using Memo.Bill.Domain.Events.Ledgers;

namespace Memo.Bill.Application.Ledgers.Commands;

[Authorize(Permissions = ApiPermission.Ledger.Create)]
[Transactional]
public record CreateLedgerCommand(string Name, int Color) : IAuthorizeableRequest<Result>;

public class CreateLedgerCommandValidator : AbstractValidator<CreateLedgerCommand>
{
    public CreateLedgerCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("账本名称不能为空");
    }
}

public class CreateAccountCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo
    ) : IRequestHandler<CreateLedgerCommand, Result>
{
    public async Task<Result> Handle(CreateLedgerCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var exist = await ledgerRepo.Select.AnyAsync(l => l.Name == request.Name && l.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("账本已存在");

        var ledger = mapper.Map<Ledger>(request);
        ledger.LedgerId = SnowFlakeUtil.NextId();
        ledger.AddDomainEvent(new CreateLedgerEvent(ledger.LedgerId, userId));
        ledger = await ledgerRepo.InsertAsync(ledger, cancellationToken);
        if (ledger.Id <= 0) throw new ApplicationException("保存账本失败");

        return Result.Success(mapper.Map<LedgerResult>(ledger));
    }
}