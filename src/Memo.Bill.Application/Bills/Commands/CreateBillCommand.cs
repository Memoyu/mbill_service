using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Create)]
[Transactional]
public record CreateBillCommand(
    BillType Type,
    long LedgerId,
    long CategoryId,
    long AccountId,
    decimal Amount,
    DateTime Date,
    string? Remark,
    string? Location,
    string? Address,
    List<long>? TagIds
) : IAuthorizeableRequest<Result>;

public class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
{
    public CreateBillCommandValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("账单类型错误");

        RuleFor(x => x.LedgerId)
           .NotEmpty()
           .WithMessage("账本Id不能为空");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("金额不能为0");
    }
}

public class CreateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<Tag> tagRepo,
    IBaseDefaultRepository<BillTag> billTagRepo
    ) : IRequestHandler<CreateBillCommand, Result>
{
    public async Task<Result> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        var ledger = await ledgerRepo.Select.Where(x => x.LedgerId == request.LedgerId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账本不存在或已删除");
        var category = await categoryRepo.Select.Where(x => x.CategoryId == request.CategoryId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("分类不存在或已删除");
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("账户不存在或已删除");

        var tags = new List<Tag>();
        if (request.TagIds != null && request.TagIds.Count > 0)
            tags = await tagRepo.Select.Where(t => request.TagIds!.Contains(t.TagId)).ToListAsync(cancellationToken) ?? [];

        var bill = mapper.Map<Billing>(request);
        bill.AddDomainEvent(new CreateBillEvent(bill));
        bill = await billRepo.InsertAsync(bill, cancellationToken);
        // 写入账单标签
        if (tags.Count > 0)
            await billTagRepo.InsertAsync(tags.Select(t => new BillTag { BillId = bill.BillId, TagId = t.TagId }), cancellationToken);

        return bill.Id > 0 ? Result.Success(bill.AccountId) : throw new ApplicationException("保存账单失败");
    }
}
