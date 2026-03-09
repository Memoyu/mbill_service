using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Accounts.Commands;

[Authorize(Permissions = ApiPermission.Account.Update)]
[Transactional]
public record UpdateAccountCommand(long AccountId, string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("账户名称不能为空");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("账户图标不能为空");
    }
}

public class UpdateBillCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<UpdateAccountCommand, Result>
{
    public async Task<Result> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entity = await accountRepo.Select.Where(x => x.AccountId == request.AccountId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账户不存在或已删除");

        var exist = await accountRepo.Select.AnyAsync(x => x.Name == request.Name && x.AccountId != request.AccountId && x.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("账户已存在");

        var update = mapper.Map<Account>(request);
        update.Id = entity.Id;
        var row = await accountRepo.UpdateAsync(update, cancellationToken);

        return row > 0 ? Result.Success() : throw new ApplicationException("更新账户失败");
    }
}