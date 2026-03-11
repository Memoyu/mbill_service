using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Update)]
[Transactional]
public record UpdateBillCommand(long BillId, long CategoryId, long AccountId, decimal Amount, BillType Type, string? Remark, string? Location, string? Address) : IAuthorizeableRequest<Result>;

public class UpdateBillCommandValidator : AbstractValidator<UpdateBillCommand>
{
    public UpdateBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("金额不能为0");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("账单类型错误");
    }
}

public class UpdateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<UpdateBillCommand, Result>
{
    public async Task<Result> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        var entity = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账单不存在或已删除");
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账户不存在或已删除");
        var category = await categoryRepo.Select.Where(x => x.CategoryId == request.CategoryId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("分类不存在或已删除");

        request.Adapt(entity);
        entity.AddDomainEvent(new UpdateBillEvent(entity));
        var row = await billRepo.UpdateAsync(entity, cancellationToken);

        entity.Category = category;
        entity.Account = account;
        return row > 0 ? Result.Success(mapper.Map<BillResult>(entity)) : throw new ApplicationException("更新账单失败");
    }
}
