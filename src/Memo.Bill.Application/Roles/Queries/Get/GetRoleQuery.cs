namespace Memo.Bill.Application.Roles.Queries.Get;

public record GetRoleQuery(long RoleId) : IAuthorizeableRequest<Result>;
