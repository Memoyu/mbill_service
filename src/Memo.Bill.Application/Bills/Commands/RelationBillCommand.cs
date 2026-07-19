namespace Memo.Bill.Application.Bills.Commands;

[Authorize(Permissions = ApiPermission.Bill.Relation)]
public record RelationBillCommand(long BillId, List<long> RelationIds) : IAuthorizeableRequest<Result>;

public class RelationBillCommandValidator : AbstractValidator<RelationBillCommand>
{
    public RelationBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("账单Id不能为空");

        RuleFor(x => x.RelationIds)
           .NotEmpty()
           .WithMessage("关联账单Id不能为空");
    }
}

public class RelationBillCommandHandler(
    IBaseDefaultRepository<Billing> billRepo,
     IBaseDefaultRepository<BillRelation> billRelationRepo
    ) : IRequestHandler<RelationBillCommand, Result>
{
    public async Task<Result> Handle(RelationBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await billRepo.Select.Where(t => t.BillId == request.BillId).FirstAsync(cancellationToken)
          ?? throw new ApplicationException("账单不存在或已删除");

        // 插入关联数据
        await billRelationRepo.InsertAsync(request.RelationIds.Select(i => new BillRelation { BillId = bill.BillId, RelationId = i }), cancellationToken);

        return Result.Success();
    }
}