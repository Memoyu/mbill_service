using Memo.Bill.Application.Roles.Common;

namespace Memo.Bill.Application.Roles.Queries;

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

public class ListRoleQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Role> roleRepo
    ) : IRequestHandler<ListRoleQuery, Result>
{
    public async Task<Result> Handle(ListRoleQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepo.Select
                .WhereIf(!string.IsNullOrWhiteSpace(request.Name), p => p.Name.Contains(request.Name!))
                .OrderByDescending(p => p.CreateTime)
                .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<RoleListResult>>(roles);
        return Result.Success(dtos);
    }
}



