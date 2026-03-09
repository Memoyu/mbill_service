namespace Memo.Bill.Application.Bills.Queries;

[Authorize(Permissions = ApiPermission.Bill.Get)]
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

public class GetBillQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<GetBillQuery, Result>
{
    public async Task<Result> Handle(GetBillQuery request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
