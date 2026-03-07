namespace Memo.Bill.Application.Users.Queries.List;

[Authorize(Permissions = ApiPermission.User.ListSelect)]
public record SelectUserQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    public string? Username { get; set; }

    public string? Nickname { get; set; }
}
