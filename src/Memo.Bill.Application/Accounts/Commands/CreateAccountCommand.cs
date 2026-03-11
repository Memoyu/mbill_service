using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Accounts.Commands;

[Authorize(Permissions = ApiPermission.Account.Create)]
[Transactional]
public record CreateAccountCommand(string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("账户名称不能为空");

        RuleFor(x => x.Icon)
          .NotEmpty()
          .WithMessage("账户图标不能为空");
    }
}

public class CreateAccountCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<CreateAccountCommand, Result>
{
    public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var exist = await accountRepo.Select.AnyAsync(x => x.Name == request.Name && x.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("账户已存在");

        var entity = mapper.Map<Account>(request);
        entity = await accountRepo.InsertAsync(entity, cancellationToken);
        if (entity.Id <= 0) throw new ApplicationException("保存账户失败");

        return Result.Success(entity.AccountId);
    }
}
