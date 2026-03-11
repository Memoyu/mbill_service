using Memo.Bill.Application.Permissions.Common;
using Memo.Bill.Application.Roles.Common;

namespace Memo.Bill.Application.Permissions.Queries;

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

public class ListPermissionQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Permission> permissionRepo,
    IBaseDefaultRepository<RolePermission> rolePermissionRepo,
    IBaseDefaultRepository<Role> roleRepo
    ) : IRequestHandler<ListPermissionQuery, Result>
{
    public async Task<Result> Handle(ListPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepo.Select
            .WhereIf(!string.IsNullOrWhiteSpace(request.Name), p => p.Name.Contains(request.Name!))
            .WhereIf(!string.IsNullOrWhiteSpace(request.Signature), p => p.Signature.Contains(request.Signature!))
            .ToListAsync(cancellationToken);

        var rolePermissions = await rolePermissionRepo.Select.ToListAsync(cancellationToken);
        var roleIds = rolePermissions.Select(rp => rp.RoleId).Distinct().ToList();
        var roles = await roleRepo.Select.Where(r => roleIds.Contains(r.RoleId)).ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<PermissionResult>>(permissions);
        foreach (var d in dtos)
        {
            var permissionRoles = rolePermissions
                .Where(rp => rp.PermissionId == d.PermissionId && roles.Any(r => r.RoleId == rp.RoleId))
                .Select(rp => mapper.Map<RoleListResult>(roles.FirstOrDefault(r => r.RoleId == rp.RoleId)!)).ToList();
            d.Roles = permissionRoles;
        }

        return Result.Success(dtos);
    }
}


