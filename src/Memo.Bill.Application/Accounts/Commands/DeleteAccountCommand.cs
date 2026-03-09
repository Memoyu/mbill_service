using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Accounts.Commands;

[Authorize(Permissions = ApiPermission.Account.Delete)]
[Transactional]
public record DeleteAccountCommand(long AccountId) : IAuthorizeableRequest<Result>;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");
    }
}

public class DeleteAccountCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<DeleteAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entity = await accountRepo.Select.Where(x => x.AccountId == request.AccountId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账户不存在或已删除");

        var row = await accountRepo.DeleteAsync(entity.AccountId, cancellationToken);
        if (row < 1) return Result.Failure("删除账户失败");
        // 删除账户为父级账户，需要删除子级账户
        if (!entity.ParentId.HasValue)
            await accountRepo.DeleteAsync(x => x.ParentId == entity.ParentId, cancellationToken);

        return Result.Success();
    }
}