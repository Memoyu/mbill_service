using Memo.Bill.Application.Ledgers.Common;
using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Ledgers.Queries;

[Authorize(Permissions = ApiPermission.Ledger.Get)]
public record GetLedgerQuery(long LedgerId) : IAuthorizeableRequest<Result>;

public class GetLedgerQueryValidator : AbstractValidator<GetLedgerQuery>
{
    public GetLedgerQueryValidator()
    {
        RuleFor(x => x.LedgerId)
            .NotEmpty()
            .WithMessage("账本Id不能为空");
    }
}

public class GetLedgerQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<LedgerUser> ledgerUserRepo,
    IBaseDefaultRepository<User> userRepo
    ) : IRequestHandler<GetLedgerQuery, Result>
{
    public async Task<Result> Handle(GetLedgerQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var ledger = await ledgerRepo.Select.Where(l => l.LedgerId == request.LedgerId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账本不存在或已删除");

        var user = await userRepo.Select.Where(l => l.UserId == ledger.CreateUserId).FirstAsync(cancellationToken);
        var ledgerUsers = await ledgerUserRepo.Select.Include(l => l.User).Where(l => l.LedgerId == ledger.LedgerId).ToListAsync(cancellationToken);
        var dto = mapper.Map<LedgerWithCreaterResult>(ledger);
        dto.Creater = mapper.Map<UserBaseResult>(user);
        dto.Users = mapper.Map<List<UserBaseResult>>(ledgerUsers);
        dto.Color = ledgerUsers.FirstOrDefault(lu => lu.UserId == userId)?.Color ?? 0;

        return Result.Success(dto);
    }
}
