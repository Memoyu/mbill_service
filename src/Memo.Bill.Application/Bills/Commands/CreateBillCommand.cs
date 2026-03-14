using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Create)]
[Transactional]
public record CreateBillCommand(long CategoryId, long AccountId, decimal Amount, BillType Type, DateTime Date, string? Remark, string? Location, string? Address) : IAuthorizeableRequest<Result>;

public class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
{
    public CreateBillCommandValidator()
    {
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

public class CreateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<CreateBillCommand, Result>
{
    public async Task<Result> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("账户不存在或已删除");
        var category = await categoryRepo.Select.Where(x => x.CategoryId == request.CategoryId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("分类不存在或已删除");

        var entity = mapper.Map<Billing>(request);
        entity.AddDomainEvent(new CreateBillEvent(entity));
        entity = await billRepo.InsertAsync(entity, cancellationToken);

        entity.Category = category;
        entity.Account = account;
        return entity.Id > 0 ? Result.Success(mapper.Map<BillResult>(entity)) : throw new ApplicationException("保存账单失败");
    }
}
