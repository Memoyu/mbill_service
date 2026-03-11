using Memo.Bill.Application.Permissions.Common;

namespace Memo.Bill.Application.Permissions.Queries;

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

public class GroupPermissionQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Permission> permissionRepo
    ) : IRequestHandler<GroupPermissionQuery, Result>
{
    public async Task<Result> Handle(GroupPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepo.Select
            .WhereIf(!string.IsNullOrWhiteSpace(request.Name), p => p.Name.Contains(request.Name!))
            .ToListAsync(cancellationToken);

        var grouos = permissions.GroupBy(p => new { p.Module, p.ModuleName }).ToList();

        var dtos = new List<GroupPermissionResult>();
        foreach (var group in grouos)
        {
            dtos.Add(new GroupPermissionResult
            {
                Module = group.Key.Module,
                ModuleName = group.Key.ModuleName,
                Permissions = group.Select(mapper.Map<PermissionResult>).ToList(),
            });
        }

        return Result.Success(dtos);
    }
}


