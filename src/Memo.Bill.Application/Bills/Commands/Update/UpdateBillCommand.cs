namespace Memo.Bill.Application.Bills.Commands.Update;

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
