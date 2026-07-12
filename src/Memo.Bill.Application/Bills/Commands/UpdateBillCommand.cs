using Memo.Bill.Domain.Events.Bills;

namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Update)]
[Transactional]
public record UpdateBillCommand(
    long BillId,
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

public class UpdateBillCommandValidator : AbstractValidator<UpdateBillCommand>
{
    public UpdateBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");

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

public class UpdateBillCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<Ledger> ledgerRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<Tag> tagRepo,
    IBaseDefaultRepository<BillTag> billTagRepo
    ) : IRequestHandler<UpdateBillCommand, Result>
{
    public async Task<Result> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("账单不存在或已删除");
        var ledger = await ledgerRepo.Select.Where(x => x.LedgerId == request.LedgerId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("账本不存在或已删除");
        var category = await categoryRepo.Select.Where(x => x.CategoryId == request.CategoryId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("分类不存在或已删除");
        var account = await accountRepo.Select.Where(x => x.AccountId == request.AccountId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("账户不存在或已删除");

        var tags = await tagRepo.Select.WhereIf(request.TagIds != null && request.TagIds.Count > 0, t => request.TagIds!.Contains(t.TagId)).ToListAsync(cancellationToken);

        request.Adapt(bill);
        bill.AddDomainEvent(new UpdateBillEvent(bill));
        var row = await billRepo.UpdateAsync(bill, cancellationToken);
        var billTags = await billTagRepo.Select.Where(bt => bt.BillId == bill.BillId).ToListAsync(cancellationToken);
        var addTags = new List<BillTag>();
        foreach (var tag in tags)
        {
            var bt = billTags.FirstOrDefault(bt => tag.TagId == bt.TagId);
            // 已存在
            if (bt != null)
            {
                billTags.Remove(bt);
                continue;
            }

            // 不存在
            addTags.Add(new BillTag { BillId = bill.BillId, TagId = tag.TagId });
        }
        // 写入账单标签
        if (addTags.Count > 0)
            await billTagRepo.InsertAsync(addTags, cancellationToken);
        // 删除账单标签
        if (billTags.Count > 0)
            await billTagRepo.DeleteAsync(billTags, cancellationToken);

        return row > 0 ? Result.Success(bill.BillId) : throw new ApplicationException("更新账单失败");
    }
}
