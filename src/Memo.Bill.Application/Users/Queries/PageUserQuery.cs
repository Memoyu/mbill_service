using Memo.Bill.Application.Roles.Common;
using Memo.Bill.Application.Users.Common;

namespace Memo.Bill.Application.Users.Queries;

[Authorize(Permissions = ApiPermission.User.Page)]
public record PageUserQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    public long? UserId { get; set; }

    public string? Username { get; set; }

    public string? Nickname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public List<long>? Roles { get; set; }

}

public class PageUserQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserRole> userRoleRepo,
    IBaseDefaultRepository<Role> roleRepo
    ) : IRequestHandler<PageUserQuery, Result>
{
    public async Task<Result> Handle(PageUserQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepo.Select
            .IncludeMany(u => u.UserRoles)
            .WhereIf(request.UserId.HasValue, u => u.UserId == request.UserId)
            .WhereIf(!string.IsNullOrWhiteSpace(request.Username), u => u.Username.Contains(request.Username!))
            .WhereIf(!string.IsNullOrWhiteSpace(request.Nickname), u => u.Nickname.Contains(request.Nickname!))
            .WhereIf(!string.IsNullOrWhiteSpace(request.Email), u => u.Email.Contains(request.Email!))
            .WhereIf(!string.IsNullOrWhiteSpace(request.PhoneNumber), u => u.PhoneNumber.Contains(request.PhoneNumber!))
            .WhereIf(request.Roles != null && request.Roles.Count > 0, u => u.UserRoles.Any(ur => request.Roles!.Contains(ur.RoleId)))
            .OrderByDescending(u => u.CreateTime)
            .ToPageListAsync(request, out var total, cancellationToken);

        var userIds = users.Select(u => u.UserId).ToList();
        var allUserRoles = await userRoleRepo.Select.Where(ur => userIds.Contains(ur.UserId)).ToListAsync(cancellationToken);
        var userRoleIds = allUserRoles.Select(ur => ur.RoleId).ToList();
        var roles = await roleRepo.Select.Where(r => userRoleIds.Contains(r.RoleId)).ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<PageUserResult>>(users);
        foreach (var dto in dtos)
        {
            var userRoles = allUserRoles
                .Where(ur => ur.UserId == dto.UserId && roles.Any(r => r.RoleId == ur.RoleId))
                .Select(ur => mapper.Map<RoleListResult>(roles.FirstOrDefault(r => r.RoleId == ur.RoleId)!))
                .ToList();
            dto.Roles = userRoles;
        }

        return Result.Success(new PaginationResult<PageUserResult>(dtos, total));
    }
}
