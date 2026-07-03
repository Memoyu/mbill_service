namespace Memo.Bill.Application.Tags.Commands;

public record SortTagDto(long TagId, int Sort);

[Authorize(Permissions = ApiPermission.Tag.Sort)]
[Transactional]
public record SortTagCommand(List<SortTagDto> Sorts) : IAuthorizeableRequest<Result>;

public class SortTagCommandValidator : AbstractValidator<SortTagCommand>
{
    public SortTagCommandValidator()
    {
        RuleFor(x => x.Sorts)
            .NotEmpty()
            .WithMessage("排序账单账户不能为空");
    }
}

public class SortTagCommandHandler(
    IBaseDefaultRepository<Tag> tagRepo
    ) : IRequestHandler<SortTagCommand, Result>
{
    public async Task<Result> Handle(SortTagCommand request, CancellationToken cancellationToken)
    {
        // 更新排序
        var tagIds = request.Sorts.Select(s => s.TagId).ToList();
        var tags = await tagRepo.Select.Where(a => tagIds.Contains(a.TagId)).ToListAsync(cancellationToken) ?? [];
        tags.ForEach(c =>
        {
            c.Sort = request.Sorts.FirstOrDefault(a => a.TagId == c.TagId)?.Sort ?? c.Sort;
        });
        var rows = await tagRepo.UpdateAsync(tags, cancellationToken);

        return rows > 0 ? Result.Success() : throw new ApplicationException("更新标签排序失败");
    }
}