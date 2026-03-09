namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Create)]
[Transactional]
public record CreateBillCommand(long CategoryId, long AssetId, decimal Amount, int Type, string? Remark, string? Location, string? Address) : IAuthorizeableRequest<Result>;

public class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
{
    public CreateBillCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("资产Id不能为空");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("金额不能为0");
    }
}

public class CreateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<CreateBillCommand, Result>
{
    public async Task<Result> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
