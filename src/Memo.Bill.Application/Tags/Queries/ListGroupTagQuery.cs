using Memo.Bill.Application.Tags.Common;

namespace Memo.Bill.Application.Tags.Queries;

[Authorize(Permissions = ApiPermission.Tag.ListGroup)]
public record ListGroupTagQuery() : IAuthorizeableRequest<Result>;

public class ListGroupAccountQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Tag> tagRepo
    ) : IRequestHandler<ListGroupTagQuery, Result>
{
    public async Task<Result> Handle(ListGroupTagQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entities = await tagRepo.Select.Where(x => x.CreateUserId == userId).ToListAsync(cancellationToken);
        var dtos = mapper.Map<List<TagGroupResult>>(entities.Where(x => !x.ParentId.HasValue).OrderBy(x => x.Sort));
        dtos.ForEach(d =>
        {
            d.Childs = mapper.Map<List<TagResult>>(entities.Where(x => x.ParentId == d.TagId).OrderBy(x => x.Sort));
        });
        return Result.Success(dtos);
    }
}
