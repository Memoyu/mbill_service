namespace Memo.Bill.Application.Permissions.Queries.Group;

[Authorize(Permissions = ApiPermission.Permission.Group)]
public record GroupPermissionQuery(
    string? Name
    ) : IAuthorizeableRequest<Result>;

public class GroupPermissionQueryValidator : AbstractValidator<GroupPermissionQuery>
{
    public GroupPermissionQueryValidator()
    {
    }
}


