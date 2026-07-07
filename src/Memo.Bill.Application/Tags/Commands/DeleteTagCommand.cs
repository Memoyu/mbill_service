namespace Memo.Bill.Application.Tags.Commands;

[Authorize(Permissions = ApiPermission.Tag.Delete)]
[Transactional]
public record DeleteTagCommand(long TagId) : IAuthorizeableRequest<Result>;

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.TagId)
            .NotEmpty()
            .WithMessage("账户Id不能为空");
    }
}

public class DeleteTagCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Tag> tagRepo
    ) : IRequestHandler<DeleteTagCommand, Result>
{
    public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entity = await tagRepo.Select.Where(x => x.TagId == request.TagId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("标签不存在或已删除");

        var row = await tagRepo.DeleteAsync(entity, cancellationToken);
        if (row < 1) return Result.Failure("删除标签失败");
        // 删除标签为父级，需要删除子级标签
        if (!entity.ParentId.HasValue)
            await tagRepo.DeleteAsync(x => x.ParentId == entity.ParentId, cancellationToken);

        return Result.Success();
    }
}