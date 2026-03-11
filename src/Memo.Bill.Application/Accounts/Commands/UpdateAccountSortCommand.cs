using Memo.Bill.Application.Accounts.Common;

namespace Memo.Bill.Application.Accounts.Commands;

[Authorize(Permissions = ApiPermission.Account.UpdateSort)]
[Transactional]
public record UpdateAccountSortCommand(List<AccountUpdateSort> UpdateSorts) : IAuthorizeableRequest<Result>;

public class SortAccountCommandValidator : AbstractValidator<UpdateAccountSortCommand>
{
    public SortAccountCommandValidator()
    {
        RuleFor(x => x.UpdateSorts)
            .NotEmpty()
            .Must(x => x.Count > 0)
            .WithMessage("排序对象不能为空");
    }
}

public class SortAccountCommandHandler(
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<UpdateAccountSortCommand, Result>
{
    public async Task<Result> Handle(UpdateAccountSortCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.UpdateSorts)
        {
            var update = await accountRepo.Select.Where(x => x.AccountId == item.AccountId).FirstAsync(cancellationToken);
            if (update == null) continue;
            update.Id = item.Sort;
            var row = await accountRepo.UpdateAsync(update, cancellationToken);
            if (row < 1) throw new ApplicationException("更新账户排序失败");
        }

        return Result.Success();
    }
}