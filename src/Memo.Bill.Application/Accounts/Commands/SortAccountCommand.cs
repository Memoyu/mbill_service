namespace Memo.Bill.Application.Accounts.Commands;

public record SortAccountDto(long AccountId, int Sort);

[Authorize(Permissions = ApiPermission.Account.Sort)]
[Transactional]
public record SortAccountCommand(List<SortAccountDto> Sorts) : IAuthorizeableRequest<Result>;

public class SortAccountCommandValidator : AbstractValidator<SortAccountCommand>
{
    public SortAccountCommandValidator()
    {
        RuleFor(x => x.Sorts)
            .NotEmpty()
            .WithMessage("排序账单账户不能为空");
    }
}

public class SortAccountCommandHandler(
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<SortAccountCommand, Result>
{
    public async Task<Result> Handle(SortAccountCommand request, CancellationToken cancellationToken)
    {
        // 更新排序
        var categoryIds = request.Sorts.Select(s => s.AccountId).ToList();
        var categories = await accountRepo.Select.Where(a => categoryIds.Contains(a.AccountId)).ToListAsync(cancellationToken) ?? [];
        categories.ForEach(c =>
        {
            c.Sort = request.Sorts.FirstOrDefault(a => a.AccountId == c.AccountId)?.Sort ?? c.Sort;
        });
        var rows = await accountRepo.UpdateAsync(categories, cancellationToken);

        return rows > 0 ? Result.Success() : throw new ApplicationException("更新账单账户排序失败");
    }
}