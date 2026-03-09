namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Update)]
[Transactional]
public record UpdateBillCommand(long CategoryId, long AssetId, decimal Amount, int Type, string? Remark, string? Location, string? Address) : IAuthorizeableRequest<Result>;

public class UpdateBillCommandValidator : AbstractValidator<UpdateBillCommand>
{
    public UpdateBillCommandValidator()
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

public class UpdateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo
    ) : IRequestHandler<UpdateBillCommand, Result>
{
    public async Task<Result> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
