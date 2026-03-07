namespace Memo.Bill.Application.Bills.Queries.Get;

public record GetBillQuery(long BillId) : IAuthorizeableRequest<Result>;

public class GetBillQueryValidator : AbstractValidator<GetBillQuery>
{
    public GetBillQueryValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");
    }
}
