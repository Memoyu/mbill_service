using Memo.Bill.Application.Tags.Common;

namespace Memo.Bill.Application.Tags.Commands;

[Authorize(Permissions = ApiPermission.Tag.Create)]
public record CreateTagCommand(long? ParentId, string Name) : IAuthorizeableRequest<Result>;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("账户名称不能为空");
    }
}

public class CreateTagCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Tag> tagRepo
    ) : IRequestHandler<CreateTagCommand, Result>
{
    public async Task<Result> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var exist = await tagRepo.Select.AnyAsync(x => x.Name == request.Name && x.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("标签已存在");
        var maxSort = await tagRepo.Select
            .Where(t => t.CreateUserId == userId)
            .Where(t => t.ParentId == request.ParentId)
            .MaxAsync(t => t.Sort + 1, cancellationToken);
        var entity = mapper.Map<Tag>(request);
        entity.Sort = maxSort;
        entity = await tagRepo.InsertAsync(entity, cancellationToken);
        if (entity.Id <= 0) throw new ApplicationException("保存标签失败");

        return Result.Success(mapper.Map<TagResult>(entity));
    }
}
