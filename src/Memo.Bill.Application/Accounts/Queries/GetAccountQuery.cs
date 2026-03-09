using Memo.Bill.Application.Accounts.Common;
using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Accounts.Queries;

/// <summary>
/// 获取账户
/// </summary>
/// <param name="AccountId">账户Id</param>
/// <param name="Parent">是否获取父级</param>
[Authorize(Permissions = ApiPermission.Account.Get)]
public record GetAccountQuery(long AccountId, bool? Parent) : IAuthorizeableRequest<Result>;

public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
{
    public GetAccountQueryValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");
    }
}

public class GetAccountQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Account> accountRepo
    ) : IRequestHandler<GetAccountQuery, Result>
{
    public async Task<Result> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entity = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("账户不存在或已删除");

        var dto = mapper.Map<AccountResult>(entity);
        if (request.Parent == true)
        {
            var parent = await accountRepo.Select.Where(x => x.AccountId == entity.ParentId).FirstAsync(cancellationToken)
                ?? throw new ApplicationException("父账户不存在或已删除");
            dto.Parent = mapper.Map<AccountResult>(parent);
        }

        return Result.Success(dto);
    }
}
