namespace Memo.Bill.Application.Roles.Queries.List;

[Authorize(Permissions = ApiPermission.Role.List)]

public record ListRoleQuery(
    string? Name
    ) : IAuthorizeableRequest<Result>;

public class ListRoleQueryValidator : AbstractValidator<ListRoleQuery>
{
    public ListRoleQueryValidator()
    {
    }
}


