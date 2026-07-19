using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Tags.Common;

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
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<BillTag> billTagRepo,
    IBaseDefaultRepository<BillRefund> billRefundRepo,
    IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo 
    ) : IRequestHandler<GetBillQuery, Result>
{
    public async Task<Result> Handle(GetBillQuery request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select
            .Include(a => a.Category)
            .Include(a => a.Account)
            .Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken) ?? throw new ApplicationException("账单不存在或已删除");

        var dto = mapper.Map<BillResult>(bill);

        // 退款总额
        dto.RefundAmount = await billRefundRepo.Select.Where(t => t.BillId == bill.BillId).SumAsync(br => br.Amount, cancellationToken);

        // 分类补全
        if (dto.Category.ParentId.HasValue)
        {
            var parentCa = await categoryRepo.Select.Where(t => dto.Category.ParentId == t.CategoryId).FirstAsync(cancellationToken);
            dto.Category.Name = parentCa == null ? dto.Category.Name : $"{parentCa.Name}-{dto.Category.Name}";
        }

        // 账户补全
        if (dto.Account.ParentId.HasValue)
        {
            var parentAc = await accountRepo.Select.Where(t => dto.Account.ParentId == t.AccountId).FirstAsync(cancellationToken);
            dto.Account.Name = parentAc == null ? dto.Account.Name : $"{parentAc.Name}-{dto.Account.Name}";
        }

        // 标签
        var tags = await billTagRepo.Select
            .Include(t => t.Tag)
            .Where(t => t.BillId == bill.BillId)
            .ToListAsync(t => t.Tag, cancellationToken);
        dto.Tags = mapper.Map<List<TagBaseResult>>(tags);

        return Result.Success(dto);
    }
}
