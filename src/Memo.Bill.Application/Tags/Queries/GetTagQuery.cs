using Memo.Bill.Application.Tags.Common;

namespace Memo.Bill.Application.Tags.Queries;

/// <summary>
/// 获取账户
/// </summary>
/// <param name="TagId">账户Id</param>
/// <param name="Parent">是否获取父级</param>
[Authorize(Permissions = ApiPermission.Tag.Get)]
public record GetTagQuery(long TagId, bool? Parent) : IAuthorizeableRequest<Result>;

public class GetTagQueryValidator : AbstractValidator<GetTagQuery>
{
    public GetTagQueryValidator()
    {
        RuleFor(x => x.TagId)
            .NotEmpty()
            .WithMessage("标签Id不能为空");
    }
}

public class GetTagQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Tag> tagRepo
    ) : IRequestHandler<GetTagQuery, Result>
{
    public async Task<Result> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;
        var entity = await tagRepo.Select.Where(x => x.TagId == request.TagId).FirstAsync(cancellationToken)
           ?? throw new ApplicationException("标签不存在或已删除");

        var dto = mapper.Map<TagWithParentResult>(entity);
        if (request.Parent == true)
        {
            var parent = await tagRepo.Select.Where(x => x.TagId == entity.ParentId).FirstAsync(cancellationToken)
                ?? throw new ApplicationException("父标签不存在或已删除");
            dto.Parent = mapper.Map<TagResult>(parent);
        }

        return Result.Success(dto);
    }
}
