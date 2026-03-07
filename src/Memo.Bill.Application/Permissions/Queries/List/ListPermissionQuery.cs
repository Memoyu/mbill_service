namespace Memo.Bill.Application.Permissions.Queries.List;

[Authorize(Permissions = ApiPermission.Permission.List)]

public record ListPermissionQuery(
    string? Name,
    string? Signature
    ) : IAuthorizeableRequest<Result>;

public class ListPermissionQueryValidator : AbstractValidator<ListPermissionQuery>
{
    public ListPermissionQueryValidator()
    {
    }
}


