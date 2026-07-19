using Memo.Bill.Application.Bills.Common;
using Memo.Bill.Application.Tags.Common;

namespace Memo.Bill.Application.Bills.Queries;

[Authorize(Permissions = ApiPermission.Bill.Related)]
public record RelatedBillQuery(long BillId) : IAuthorizeableRequest<Result>;

public class RelatedBillQueryValidator : AbstractValidator<RelatedBillQuery>
{
    public RelatedBillQueryValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");
    }
}

public class RelatedBillQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Billing> billRepo,
    IBaseDefaultRepository<BillRelation> billRelationRepo,
        IBaseDefaultRepository<Category> categoryRepo,
    IBaseDefaultRepository<Account> accountRepo,
    IBaseDefaultRepository<BillTag> billTagRepo
    ) : IRequestHandler<RelatedBillQuery, Result>
{
    public async Task<Result> Handle(RelatedBillQuery request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("账单不存在或已删除");

        var relatedBills = await billRelationRepo.Select
            .Include(br => br.RelatedBill)
            .Where(br => br.BillId == bill.BillId)
            .ToListAsync(br => br.RelatedBill, cancellationToken);

        var billIds = new HashSet<long>();
        var parCaIds = new HashSet<long>();
        var parAcIds = new HashSet<long>();

        foreach (var rb in relatedBills)
        {
            billIds.Add(rb.BillId);

            if (rb.Category.ParentId.HasValue)
                parCaIds.Add(rb.Category.ParentId.Value);
            if (rb.Account.ParentId.HasValue)
                parAcIds.Add(rb.Account.ParentId.Value);
        }

        var parCas = await categoryRepo.Select.Where(t => parCaIds.Contains(t.CategoryId)).ToListAsync(cancellationToken);
        var parAcs = await accountRepo.Select.Where(t => parAcIds.Contains(t.AccountId)).ToListAsync(cancellationToken);
        var tags = await billTagRepo.Select.Include(t => t.Tag).Where(t => billIds.Contains(t.BillId)).ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<BillResult>>(relatedBills);
        dtos.ForEach(b =>
        {
            var parAc = parAcs.FirstOrDefault(c => c.AccountId == b.Account.ParentId);
            var parCa = parCas.FirstOrDefault(c => c.CategoryId == b.Category.ParentId);
            b.Category.Name = parCa == null ? b.Category.Name : $"{parCa.Name}-{b.Category.Name}";
            b.Account.Name = parAc == null ? b.Account.Name : $"{parAc.Name}-{b.Account.Name}";
            b.Tags = [.. tags.Where(t => t.BillId == b.BillId).Select(t => mapper.Map<TagBaseResult>(t.Tag))];
        });

        return Result.Success(dtos);
    }
}