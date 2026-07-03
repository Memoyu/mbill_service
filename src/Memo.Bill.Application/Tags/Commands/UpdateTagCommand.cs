namespace Memo.Bill.Application.Tags.Commands;

[Authorize(Permissions = ApiPermission.Tag.Update)]
public record UpdateTagCommand(long TagId, string Name, long? ParentId) : IAuthorizeableRequest<Result>;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.TagId)
            .NotEmpty()
            .WithMessage("标签Id不能为空");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("标签名称不能为空");
    }
}

public class UpdateTagCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Tag> tagRepo
    ) : IRequestHandler<UpdateTagCommand, Result>
{
    public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entity = await tagRepo.Select.Where(x => x.TagId == request.TagId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("标签不存在或已删除");

        var exist = await tagRepo.Select.AnyAsync(x => x.Name == request.Name && x.TagId != request.TagId && x.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("标签已存在");

        request.Adapt(entity);
        var row = await tagRepo.UpdateAsync(entity, cancellationToken);

        return row > 0 ? Result.Success() : throw new ApplicationException("更新标签失败");
    }
}