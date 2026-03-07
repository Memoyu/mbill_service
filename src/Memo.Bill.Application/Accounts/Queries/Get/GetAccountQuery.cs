namespace Memo.Bill.Application.Accounts.Queries.Get;

public record GetAccountQuery(long AccountId) : IAuthorizeableRequest<Result>;

public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
{
    public GetAccountQueryValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");
    }
}
