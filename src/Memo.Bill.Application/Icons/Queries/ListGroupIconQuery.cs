namespace Memo.Bill.Application.Icons.Queries;

[Authorize(Permissions = ApiPermission.Icon.ListGroup)]
public record ListGroupIconQuery() : IAuthorizeableRequest<Result>;

public class ListGroupIconQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Icon> iconRepo
    ) : IRequestHandler<ListGroupIconQuery, Result>
{
    public async Task<Result> Handle(ListGroupIconQuery request, CancellationToken cancellationToken)
    {
     
        return Result.Success();
    }
}
