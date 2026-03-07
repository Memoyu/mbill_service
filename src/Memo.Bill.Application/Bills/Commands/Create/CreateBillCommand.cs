namespace Memo.Bill.Application.Bills.Commands.Create;

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
